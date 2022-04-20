using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChemSolution_re_API.Data;
using ChemSolution_re_API.Entities;
using Microsoft.AspNetCore.Authorization;
using ChemSolution_re_API.DTO.Request;
using AutoMapper;
using ChemSolution_re_API.DTO.Response;

namespace ChemSolution_re_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RequestsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestResponse>>> GetRequests()
        {
            var response = await _context.Requests.ToListAsync();
            return Ok(_mapper.Map< IEnumerable<RequestResponse>>(response));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestResponse>> GetRequest(Guid id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return _mapper.Map<RequestResponse>(request);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(Guid id, UpdateRequest model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var request = await _context.Requests.FindAsync(id);
            if(request == null)
            {
                return NotFound();
            }
            
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

            if(request == null) { return NotFound(); }

            try
            {
                var tmpStatus = Enum.Parse<Status>(status);
                request.Status = tmpStatus;
                await _context.SaveChangesAsync();

                switch (tmpStatus)
                {
                    case Status.Accepted:
                        request.User.Honesty += 10;
                        break;
                    case Status.Rejected:
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

        // POST: api/Requests
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostRequest(CreateRequest model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id.ToString() == User.Identity!.Name);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var request = _mapper.Map<Request>(model);
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
