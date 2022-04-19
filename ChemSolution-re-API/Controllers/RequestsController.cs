using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChemSolution_re_API.Data;
using ChemSolution_re_API.Entities;
using Microsoft.AspNetCore.Authorization;
using ChemSolution_re_API.DTO.Request;

namespace ChemSolution_re_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly DataContext _context;

        public RequestsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(Guid id)
        {
            var request = await _context.Requests.SingleOrDefaultAsync(x => x.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(Guid id, UpdateRequest model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var request = await _context.Requests.SingleOrDefaultAsync(x => x.Id == id);
            if(request == null)
            {
                return NotFound();
            }
            
            request.DateTimeSended = model.DateTimeSended;
            request.Theme = model.Theme;
            request.Text = model.Text;
            request.Status = Enum.Parse<Status>(model.Status);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        [HttpPut("set/status/{status}/{requestId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetStatus(string status, Guid requestId)
        {
            var request =await _context.Requests
                .Include(r => r.User)
                .SingleOrDefaultAsync(r => r.Id == requestId);

            if (request != null)
            {
                try
                {
                    var tmpStatus = Enum.Parse<Status>(status);
                    request.Status = tmpStatus;
                    await _context.SaveChangesAsync();
                    switch (tmpStatus)
                    {
                        case Status.Rejected:
                            request.User.Honesty += 10;
                            break;
                        case Status.Accepted:
                            if (request.User.Honesty >= 0)
                            {
                                request.User.Honesty -= 10;
                            }
                            break;
                    }

                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Problem();
        }

        // POST: api/Requests
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostRequest(Request request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id.ToString() == User.Identity!.Name);
            if (user == null)
            {
                return NotFound("User not found");
            }

            request.UserId = user.Id;

            await _context.Requests.AddAsync(request);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRequest(Guid id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(Guid id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
