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
    public class BlogPostsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BlogPostsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/BlogPosts/ForAdmin
        [HttpGet("ForAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BlogPostResponse>>> GetBlogPostsForAdmin()
        {
            var response = await _context.BlogPosts.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<BlogPostResponse>>(response));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostResponse>>> GetBlogPosts()
        {
            var response = await _context.BlogPosts
                .Where(x => !x.IsLocked)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<BlogPostResponse>>(response));
        }

        [HttpGet("Search/{search}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<BlogPostResponse>>> GetElementsBySearchString(string search)
        {
            var response = await _context.BlogPosts
                .Where(x => !x.IsLocked && (x.Title.Contains(search) || x.Information.Contains(search)))
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<BlogPostResponse>>(response));
        }

        // GET: api/BlogPosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPostResponse>> GetBlogPost(Guid id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            return _mapper.Map<BlogPostResponse>(blogPost);
        }

        // PUT: api/BlogPosts/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutBlogPost(Guid id, UpdateBlogPost model)
        {
            if (id != model.BlogPostId)
            {
                return BadRequest();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            _mapper.Map(model, blogPost);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogPostExists(id))
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

        // POST: api/BlogPosts
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BlogPostResponse>> PostBlogPost(CreateBlogPost model)
        {
            var blogPost = _mapper.Map<BlogPost>(model);
            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBlogPosts),
                new { id = blogPost.BlogPostId },
                _mapper.Map<BlogPostResponse>(blogPost));
        }

        // DELETE: api/BlogPosts/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBlogPost(Guid id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogPostExists(Guid id)
        {
            return _context.BlogPosts.Any(e => e.BlogPostId == id);
        }
    }
}
