using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Dtos.UpdateDto;
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
            var genre = await _genresService.GetGenresAsync();
            if (!genre.IsSuccess)
                return BadRequest(genre.ErrorMessage);

            return Ok(genre.Value);
        }



        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateGenreDto dto)
        {
            var genre = await _genresService.AddAsync(dto.Name);
            if (!genre.IsSuccess)
                return BadRequest(genre.ErrorMessage);
            return Ok(genre.Value);  

        }

        [HttpPut("Update Genre")]
        public async Task<IActionResult> UpdateAsync(GenreDtoUpdate dto)
        {
            var genre = await _genresService.UpdateAsync(dto);
            if (!genre.IsSuccess)
                return BadRequest(genre.ErrorMessage);
            return Ok(genre.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genresService.DeleteAsync(id);
            if (!genre.IsSuccess)
                return BadRequest(genre.ErrorMessage);
            return Ok(genre.Value);

        }
    }
}
