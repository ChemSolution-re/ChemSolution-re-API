﻿using AutoMapper;
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
    public class ElementsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ElementsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<ElementResponse>>> GetElements()
        {
            var userId = HttpContext.User.Identity!.Name;

            var response = await _context.Elements
                .Include(p => p.Materials)
                .Include(p => p.ElementValences)
                .Include(p => p.Users)
                .Where(p => !p.IsLocked || p.Users.Any(x => x.Id.ToString() == userId))
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ElementResponse>>(response));
        }

        // GET: api/Elements/ForAdmin
        [HttpGet("ForAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ElementResponse>>> GetElementsForAdmin()
        {
            var response = await _context.Elements
                .Include(p => p.Materials)
                .Include(p => p.ElementValences)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ElementResponse>>(response));
        }

        [HttpGet("Search/{search}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<ElementResponse>>> GetElementsBySearchString(string search)
        {
            var userId = HttpContext.User.Identity!.Name;

            var response = await _context.Elements
                .Include(p => p.Materials)
                .Include(p => p.ElementValences)
                .Where(p => !p.IsLocked || (p.Users.Any(x => x.Id.ToString() == userId) && (p.Symbol.Contains(search) || p.Info.Contains(search))))
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ElementResponse>>(response));
        }

        // GET: api/Elements/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ElementResponse>> GetElement(int id)
        {
            var element = await _context.Elements
                .Include(p => p.Materials)
                .Include(p => p.ElementValences)
                .SingleOrDefaultAsync(x => x.ElementId == id);

            if (element == null)
            {
                return NotFound();
            }

            return _mapper.Map<ElementResponse>(element);
        }

        // PUT: api/Elements/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutElement(int id, CreateElement model)
        {
            if (id != model.ElementId)
            {
                return BadRequest();
            }

            var element = await _context.Elements
                .Include(x => x.ElementValences)
                .SingleOrDefaultAsync(x => x.ElementId == id);

            _mapper.Map(model, element);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElementExists(id))
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

        // POST: api/Elements
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Element>> PostElement(CreateElement model)
        {
            var element = _mapper.Map<Element>(model);

            _context.Elements.Add(element);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ElementExists(element.ElementId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetElement", new { id = element.ElementId }, _mapper.Map<ElementResponse>(element));
        }

        // DELETE: api/Elements/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteElement(int id)
        {
            var element = await _context.Elements.FindAsync(id);
            if (element == null)
            {
                return NotFound();
            }

            _context.Elements.Remove(element);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ElementExists(int id)
        {
            return _context.Elements.Any(e => e.ElementId == id);
        }
    }
}
