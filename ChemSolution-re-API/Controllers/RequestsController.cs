using AutoMapper;
using ChemSolution_re_API.Data;
using ChemSolution_re_API.DTO.Request;
using ChemSolution_re_API.DTO.Response;
using ChemSolution_re_API.Entities;
using ChemSolution_re_API.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestResponse>>> GetRequests()
        {
            var response = await _context.Requests.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<RequestResponse>>(response));
        }

        // GET: api/Requests
        [Authorize(Roles = "User")]
        [HttpGet("OwnRequests")]
        public async Task<ActionResult<IEnumerable<RequestResponse>>> GetOwnRequests()
        {
            var response = await _context.Requests.Where(x => x.UserId.ToString() == User.Identity!.Name).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<RequestResponse>>(response));
        }

        [Authorize(Roles = "Admin")]
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

        [HttpPut("set/status/{requestId}/{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetStatus(string status, Guid requestId)
        {
            var request = await _context.Requests
                .Include(r => r.User)
                .SingleOrDefaultAsync(r => r.Id == requestId);

            if (request == null) { return NotFound(); }

            try
            {
                request.Status = Enum.Parse<Status>(status);

                switch (request.Status)
                {
                    case Status.Accepted:
                        request.User!.Honesty += 10;
                        break;
                    case Status.Rejected:
                        if (request.User!.Honesty >= 0)
                        {
                            request.User.Honesty -= 10;
                        }
                        break;
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Requests
        [Authorize(Roles = "User")]
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
    }
}
