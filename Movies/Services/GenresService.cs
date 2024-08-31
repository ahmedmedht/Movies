
using Microsoft.EntityFrameworkCore;

namespace Movies.Services
{
    public class GenresService : IGenresService
    {
        private readonly ApplictionDbContext _context;

        public GenresService(ApplictionDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> AddAsync(Genre genre)
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public Genre DeleteAsync(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<Genre> GetById(byte id)
        {
            return await _context.GenreSet.SingleOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            return await _context.GenreSet.OrderBy(g => g.Name).ToListAsync();
        }

        public Genre UpdateAsync(Genre genre)
        {
           _context.Update(genre);
            _context.SaveChanges();
            return genre;
        }
    }
}
