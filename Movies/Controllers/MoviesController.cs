using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Movies.Dtos;
using Movies.Dtos.ShowData;
using Movies.Dtos.UpdateDto;
using Movies.Model;
using Movies.Services;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        [HttpGet("GetAllMovies")]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _moviesService.GetAllMovie();
            if (!movies.IsSuccess) {
                return BadRequest(movies.ErrorMessage);
            }
            var moviesShow = new List<MovieDtoShow>();
            foreach (var movie in movies.Value) {
                var checkMovie = await _moviesService.ShowMovieData(movie);

                moviesShow.Add(checkMovie.Value);
            }

            return Ok(moviesShow);
        }

        [HttpGet("GetMovieById{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie= await _moviesService.GetById(id);
            if (!movie.IsSuccess) 
                return BadRequest(movie.ErrorMessage);
            var movieShow = await _moviesService.ShowMovieData(movie.Value);

            if (!movieShow.IsSuccess && movieShow.ErrorMessage.Length > 0)
                return Ok(movieShow.ErrorMessage);

            
                
            return Ok(movieShow.Value);
        }


        [HttpPost("AddMovie")]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            var movie = await _moviesService.AddMovieAsync(dto);
            if (!movie.IsSuccess)
                return BadRequest(movie.ErrorMessage);

            var movieShow = await _moviesService.ShowMovieData(movie.Value);
            if (!movieShow.IsSuccess)
                movieShow.Value.ImageData = [0];


            return Ok(movieShow.Value);
        }

        [HttpDelete("DeleteMovie")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var delMovie = await _moviesService.DeleteMovie(id);
            if(!delMovie.IsSuccess)
                return BadRequest(delMovie.ErrorMessage);

            var movieShow = await _moviesService.ShowMovieData(delMovie.Value);
            if (!movieShow.IsSuccess)
                movieShow.Value.ImageData = [0];


            return Ok(movieShow.Value);

        }

        [HttpPut("UpdateMovie")]
        public async Task<IActionResult> UpdateAsync(MovieDtoUpdate dto)
        {
            var movie = await _moviesService.UpdateMovie(dto);
            if (!movie.IsSuccess)
                return BadRequest(movie.ErrorMessage);

            var movieShow = await _moviesService.ShowMovieData(movie.Value);
            if (!movieShow.IsSuccess)
                movieShow.Value.ImageData = [0];


            return Ok(movieShow.Value);
        }

    }
}
