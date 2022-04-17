using AutoMapper;
using ChemSolution_re_API.Data;
using ChemSolution_re_API.DTO;
using ChemSolution_re_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChemSolution_re_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialGroupsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MaterialGroupsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/MaterialGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialGroupDTO>>> GetMaterialGroups()
        {
            var response = await _context.MaterialGroups.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<MaterialGroupDTO>>(response));
        }

        // GET: api/MaterialGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialGroupDTO>> GetMaterialGroup(Guid id)
        {
            var materialGroup = await _context.MaterialGroups.FindAsync(id);

            if (materialGroup == null)
            {
                return NotFound();
            }

            return _mapper.Map<MaterialGroupDTO>(materialGroup);
        }

        // PUT: api/MaterialGroups/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutMaterialGroup(Guid id, MaterialGroupDTO materialGroupDTO)
        {
            if (id != materialGroupDTO.Id)
            {
                return BadRequest();
            }

            var materialGroup = await _context.MaterialGroups.FindAsync(id);
            if (materialGroup == null)
            {
                return NotFound();
            }

            materialGroup.GroupName = materialGroupDTO.GroupName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialGroupExists(id))
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

        // POST: api/MaterialGroups
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MaterialGroupDTO>> PostMaterialGroup(MaterialGroupDTO materialGroupDTO)
        {
            var materialGroup = _mapper.Map<MaterialGroup>(materialGroupDTO);
            _context.MaterialGroups.Add(materialGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMaterialGroups),
                new { id = materialGroup.Id },
                _mapper.Map<MaterialGroupDTO>(materialGroup));
        }

        // DELETE: api/MaterialGroups/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMaterialGroup(Guid id)
        {
            var materialGroup = await _context.MaterialGroups.FindAsync(id);
            if (materialGroup == null)
            {
                return NotFound();
            }

            _context.MaterialGroups.Remove(materialGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaterialGroupExists(Guid id)
        {
            return _context.MaterialGroups.Any(e => e.Id == id);
        }
    }
}
