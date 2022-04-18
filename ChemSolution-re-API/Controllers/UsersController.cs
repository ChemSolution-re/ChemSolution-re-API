using ChemSolution_re_API.Data;
using ChemSolution_re_API.DTO.Request;
using ChemSolution_re_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChemSolution_re_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var response = await _context.Users
                .Include(p => p.BlogPosts)
                .Include(p => p.Requests)
                .Include(p => p.ResearchHistorys)
                .Include(p => p.Elements)
                .Include(p => p.Achievements)
                .ToListAsync();

            response.ForEach(p => p.PasswordHash = string.Empty);

            return response;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var id = HttpContext.User.Identity?.Name;

            var user = await _context.Users
                .Include(p => p.BlogPosts)
                .Include(p => p.Requests)
                .ThenInclude(r => r.Status)
                .Include(p => p.ResearchHistorys)
                .Include(p => p.Elements)
                .Include(p => p.Achievements)
                .FirstAsync(u => u.Id.ToString() == id);

            user.PasswordHash = string.Empty;

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(Guid id, UpdateUser model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.DateOfBirth = model.DateOfBirth;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
        //}

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("liked/add/{postId}")]
        [Authorize]
        public async Task<IActionResult> AddLikedPost(Guid postId)
        {
            var id = HttpContext.User.Identity?.Name;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var post = await _context.BlogPosts.FindAsync(postId);

            if (post != null)
            {
                try
                {
                    user.BlogPosts.Add(post);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch
                {
                    return Conflict();
                }
            }
            return NotFound("BlogPost not found");
        }

        [HttpPost("liked/remove/{postId}")]
        [Authorize]
        public async Task<IActionResult> RemoveLikedPost(Guid postId)
        {
            var id = HttpContext.User.Identity?.Name;

            var user = await _context.Users
                .Include(p => p.BlogPosts)
                .FirstOrDefaultAsync(u => u.Id.ToString() == id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var post = await _context.BlogPosts.FindAsync(postId);
            if (post != null)
            {
                try
                {
                    user.BlogPosts.Remove(post);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch
                {
                    return Conflict();
                }
            }
            return NotFound("BlogPost not found");
        }

        [HttpGet("rating")]
        public async Task<ActionResult<IEnumerable<User>>> GetRating()
        {
            var response = await _context.Users.OrderByDescending(u => u.Rating).ToListAsync();
            response.ForEach(p => p.PasswordHash = string.Empty);
            return response;
        }
        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
