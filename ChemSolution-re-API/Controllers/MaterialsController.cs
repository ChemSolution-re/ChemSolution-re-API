using AutoMapper;
using ChemSolution_re_API.Data;
using ChemSolution_re_API.DTO.Request;
using ChemSolution_re_API.DTO.Response;
using ChemSolution_re_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChemSolution_re_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MaterialsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Materials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialResponse>>> GetMaterials()
        {
            var response = await _context.Materials
                .Include(x => x.ElementMaterials)
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<MaterialResponse>>(response));
        }

        // GET: api/Materials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialResponse>> GetMaterial(Guid id)
        {
            var material = await _context.Materials
                .Include(x => x.ElementMaterials)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (material == null)
            {
                return NotFound();
            }

            return _mapper.Map<MaterialResponse>(material);
        }

        // PUT: api/Materials/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial(Guid id, UpdateMaterial model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var material = await _context.Materials
                .Include(x => x.ElementMaterials)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (material == null)
            {
                return NotFound();
            }

            _mapper.Map(model, material);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Materials
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MaterialResponse>> PostMaterial(CreateMaterial model)
        {
            var material = _mapper.Map<Material>(model);

            var elementMaterials = material.ElementMaterials;
            material.ElementMaterials = new();

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
            await _context.ElementMaterials.AddAsync(new ElementMaterial { ElementId = 1, Amount = 2, MaterialId = material.Id });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaterial", new { id = material.Id }, _mapper.Map<MaterialResponse>(material));
        }

        // DELETE: api/Materials/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(Guid id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaterialExists(Guid id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(SearchMaterialRequest searchRequest)
        {
            var material = _context.Materials
                .Include(p => p.ElementMaterials).AsEnumerable()
                .SingleOrDefault(m => IsEqualMaterials(m, searchRequest));

            if (material != null)
            {
                var id = HttpContext.User.Identity?.Name;

                var user = await _context.Users
                   .Include(u => u.ResearchHistorys)
                   .Include(u => u.Achievements)
                   .SingleOrDefaultAsync(u => u.Id.ToString() == id);
                if (user != null)
                {
                    bool isNew = user.ResearchHistorys.SingleOrDefault(m => m.MaterialId == material.Id) == null;
                    var achievementsId = new List<Guid>();
                    if (isNew)
                    {
                        user.ResearchHistorys.Add(
                        new ResearchHistory()
                        {
                            UserId = user.Id,
                            MaterialId = material.Id,
                            Material = material,
                            DateTime = DateTime.Today
                        });
                        await _context.SaveChangesAsync();
                        achievementsId = await ComplateAchivmentsAsync(user);
                    }
                    return new JsonResult(new
                    {
                        ResultMaterialId = material.Id,
                        IsNew = isNew,
                        NewAchievementsId = achievementsId,
                        Formula = material.Formula,
                        Info = material.Info
                    });
                }
                return new JsonResult(new
                {
                    ResultMaterialId = material.Id,
                    IsNew = true,
                    NewAchievementsId = new List<Guid>(),
                    Formula = material.Formula,
                    Info = material.Info
                });
            }
            return NotFound();
        }

        private bool IsEqualMaterials(Material material, SearchMaterialRequest searchRequest)
        {
            var tmpU1 = material.ElementMaterials
                .Select(e => (ElementId: e.ElementId, Amount: e.Amount))
                .ToList();
            var tmpU2 = searchRequest.Value
                .Select(rm => (ElementId: rm.ElementId, Amount: rm.Amount))
                .ToList();

            return (tmpU1.Count == tmpU2.Count) ? !tmpU1.Except(tmpU2).Any() : false;
        }

        private async Task<List<Guid>> ComplateAchivmentsAsync(User user)
        {
            var tmpU1 = user.Materials.GroupBy(u => u.MaterialGroup)
                .Select(ug => (Key: ug.Key, Amount: ug.Count())).ToList();
            var tmpU2 = _context.Achievements.AsEnumerable()
                .Select(a => (Key: a.MaterialGroup, Amount: a.CountGoal)).ToList();
            var res = tmpU1.Intersect(tmpU2);

            var achievementsId = new List<Guid>();

            foreach (var (Key, Amount) in res)
            {
                var tmpAchievement =
                    await _context.Achievements.SingleOrDefaultAsync(a => a.MaterialGroup == Key && a.CountGoal == Amount);
                if (tmpAchievement != null)
                {
                    if (!user.Achievements.Contains(tmpAchievement))
                    {
                        achievementsId.Add(tmpAchievement.Id);
                        user.Balance += tmpAchievement.MoneyReward;
                        user.Rating += tmpAchievement.RatingReward;
                        user.Achievements.Add(tmpAchievement);
                    }
                }
            }
            await _context.SaveChangesAsync();

            return achievementsId;
        }
    }
}
