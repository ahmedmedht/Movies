
using Microsoft.EntityFrameworkCore;
using Movies.Dtos.UpdateDto;

namespace Movies.Services.Imp
{
    public class GenresService : IGenresService
    {
        private readonly ApplictionDbContext _context;

        public GenresService(ApplictionDbContext context)
        {
            _context = context;
        }

        public async Task<Response<Genre>> AddAsync(string genreType)
        {
            var res = new Response<Genre>();
            try
            {
                var genre = new Genre()
                {
                    Name = genreType,
                };
                await _context.AddAsync(genre);
                _context.SaveChanges();

                res.IsSuccess = true;
                res.Value = genre;
                return res;

            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
                res.IsSuccess = false;

                return res;
            }
        }

        public async Task<Response<Genre>> DeleteAsync(byte id)
        {
            var genre = await GetById(id);
            if (!genre.IsSuccess)
                return genre;
            try
            {
                _context.Remove(genre.Value);
                _context.SaveChanges();

                return genre;
            }
            catch (Exception ex)
            {
                genre.ErrorMessage = ex.Message;
                genre.IsSuccess = false;
                return genre;
            }
        }

        public async Task<Response<Genre>> GetById(byte id)
        {
            var res = new Response<Genre>();
            try
            {
                res.Value = await _context.GenreSet.SingleOrDefaultAsync(g => g.Id == id);
                res.IsSuccess = true;
                if (res.Value == null)
                {
                    res.IsSuccess = false;
                    res.ErrorMessage = "Genre didn't found.";
                }

                return res;

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;

                return res;

            }

        }

        public async Task<Response<IEnumerable<Genre>>> GetGenresAsync()
        {
            var res = new Response<IEnumerable<Genre>>();
            try
            {
                var genres = await _context.GenreSet.OrderBy(g => g.Name).ToListAsync();
                res.IsSuccess = true;
                res.Value = genres;

                return res;

            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;

                return res;
            }
        }

        public async Task<Response<Genre>> UpdateAsync(GenreDtoUpdate dto)
        {
            var genre = await GetById(dto.Id);
            if (!genre.IsSuccess)
                return genre;

            genre.Value.Name = dto.Genre;

            try
            {
                _context.Update(genre.Value);
                _context.SaveChanges();
                return genre;
            }
            catch (Exception ex)
            {

                genre.IsSuccess = false;
                genre.ErrorMessage = ex.Message;
                return genre;
            }
        }
    }
}
