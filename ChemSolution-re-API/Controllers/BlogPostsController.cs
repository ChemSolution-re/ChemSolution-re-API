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
    public class BlogPostsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BlogPostsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/BlogPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostDTO>>> GetBlogPosts()
        {
            var response = await _context.BlogPosts.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<BlogPostDTO>>(response));
        }

        // GET: api/BlogPosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPostDTO>> GetBlogPost(Guid id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            return _mapper.Map<BlogPostDTO>(blogPost);
        }

        // PUT: api/BlogPosts/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutBlogPost(Guid id, BlogPostDTO blogPostDTO)
        {
            if (id != blogPostDTO.BlogPostId)
            {
                return BadRequest();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            blogPost.Title = blogPostDTO.Title;
            blogPost.BlogPostCategory = Enum.Parse<BlogPostCategory>(blogPostDTO.BlogPostCategory);
            blogPost.Information = blogPostDTO.Information;
            blogPost.Image = blogPostDTO.Image;
            blogPost.IsLocked = blogPostDTO.IsLocked;

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
        public async Task<ActionResult<BlogPostDTO>> PostBlogPost(BlogPostDTO blogPostDTO)
        {
            var blogPost = _mapper.Map<BlogPost>(blogPostDTO);
            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBlogPosts),
                new { id = blogPost.BlogPostId },
                _mapper.Map<BlogPostDTO>(blogPost));
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
