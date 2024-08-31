using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplictionDbContext _context;

        private new List<string> _allowedExtenstions = new() { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1024 * 1024;



        public MoviesController(ApplictionDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movie = await _context.Movies.Include(c => c.Genre).ToListAsync();
            return Ok(movie);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.Include(c => c.Genre).SingleOrDefaultAsync(m => m.Id == id);
            return Ok(movie);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByIdGenreAsync(byte genreId)
        {
            var movie = await _context.Movies.Where(m => m.GenreId == genreId).Include(c => c.Genre).ToListAsync();
            return Ok(movie);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is requrid!");
            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed");

            if (_maxAllowedPosterSize < dto.Poster.Length )
                return BadRequest("Max size 1 mb");

            var isValidGenre = await _context.GenreSet.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid genere ID");


            using var dataStream = new MemoryStream();

            await dto.Poster.CopyToAsync(dataStream);

            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster = dataStream.ToArray(),
                Rate = dto.Rate,
                StoreLine = dto.StoreLine,
                Year = dto.Year
            };

            await _context.AddAsync(movie);
            _context.SaveChanges();

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return BadRequest($"No movie found with ID {id}");
            _context.Remove(movie);
            _context.SaveChanges();
            return Ok(movie);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,MovieDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return BadRequest($"No movie found with ID {id}");
            var isValidGenre = await _context.GenreSet.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest("Invalid genere ID");

            if (dto.Poster != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed");

                if (_maxAllowedPosterSize < dto.Poster.Length)
                    return BadRequest("Max size 1 mb");

                using var dataStream = new MemoryStream();

                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();

            }
            else {
                movie.Poster = movie.Poster;
            }
            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.StoreLine = dto.StoreLine;
            movie.Rate = dto.Rate;
            movie.GenreId = dto.GenreId;

            _context.SaveChanges();
            return Ok(movie);

        }



        

    }
}
