using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperHeroAPI.Controllers.Data;
using SuperHeroAPI.Controllers.DTO;
using SuperHeroAPI.Controllers.Entities;
using SuperHeroAPI.Controllers.Metadata;
using System.Text.Json;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _context;

        public SuperHeroController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllHeroes([FromQuery] IndexDTO indexDto)
        {
            var query = _context.SuperHeroes.AsQueryable();
            if (!string.IsNullOrEmpty(indexDto.search))
            {
                query = query.Where(superHero => superHero.Name.Contains(indexDto.search));
            };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)indexDto.pageSize);

            var heroes = await query
                .Skip((indexDto.pageNumber - 1) * indexDto.pageSize)
                .Take(indexDto.pageSize)
                .ToListAsync();

            var paginationMetadata = new PaginationMetadata
            {
                TotalCount = totalItems,
                PageSize = indexDto.pageSize,
                CurrentPage = indexDto.pageNumber,
                TotalPages = totalPages,
            };

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(new { SuperHero = heroes, Pagination = paginationMetadata});
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetHero(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero == null)
                return NotFound("Hero not found");

            return Ok(hero);
        }
    }
}
