    namespace Movies.Services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetGenresAsync();
        Task<Genre> AddAsync(Genre genre);
        Task<Genre> GetById(byte id);
        Genre UpdateAsync(Genre genre);
        Genre DeleteAsync(Genre genre);

    }
}
