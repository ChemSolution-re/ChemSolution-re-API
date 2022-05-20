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
    public class AchievementsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AchievementsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Achievements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AchievementResponse>>> GetAchievements()
        {
            var response = await _context.Achievements.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<AchievementResponse>>(response));
        }

        // GET: api/Achievements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AchievementResponse>> GetAchievement(Guid id)
        {
            var achievement = await _context.Achievements.FindAsync(id);

            if (achievement == null)
            {
                return NotFound();
            }

            return _mapper.Map<AchievementResponse>(achievement);
        }

        [Authorize(Roles = "User")]
        [HttpGet("OwnAchievements")]
        public async Task<ActionResult<IEnumerable<AchievementResponse>>> GetOwnAchievements()
        {
            var user = await _context.Users
                .Include(x => x.Achievements)
                .SingleOrDefaultAsync(x => x.Id.ToString() == User.Identity!.Name);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<AchievementResponse>>(user.Achievements));
        }

        // PUT: api/Achievements/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAchievement(Guid id, UpdateAchievement model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement == null)
            {
                return NotFound();
            }

            _mapper.Map(model, achievement);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AchievementExists(id))
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

        // POST: api/Achievements
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Achievement>> PostAchievement(CreateAchievement model)
        {
            var achievement = _mapper.Map<Achievement>(model);
            _context.Achievements.Add(achievement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAchievement", new { id = achievement.Id }, _mapper.Map<AchievementResponse>(achievement));
        }

        // DELETE: api/Achievements/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAchievement(Guid id)
        {
            var achievement = await _context.Achievements.FindAsync(id);
            if (achievement == null)
            {
                return NotFound();
            }

            _context.Achievements.Remove(achievement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AchievementExists(Guid id)
        {
            return _context.Achievements.Any(e => e.Id == id);
        }
    }
}
