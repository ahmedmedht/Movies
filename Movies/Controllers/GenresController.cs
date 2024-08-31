using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Services;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresService _genresService;

        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            
            return Ok(await _genresService.GetGenresAsync());
        }



        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateGenreDto dto)
        {
            var genre =new Genre { Name= dto.Name};
           await _genresService.AddAsync(genre);
            return Ok(genre);  

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromForm] CreateGenreDto dto )
        {
            var genre = await _genresService.GetById(id);

            if (genre == null) 
                return NotFound($"No genre was found with ID: {id}");
            genre.Name = dto.Name;
            _genresService.UpdateAsync(genre);
            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id, [FromBody] CreateGenreDto dto)
        {
            var genre = await _genresService.GetById(id);
            if (genre == null)
                return NotFound($"No genre was found with ID: {id}");
            _genresService.DeleteAsync(genre);
            return Ok(genre);

        }
    }
}
