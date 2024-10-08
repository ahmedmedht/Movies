using Movies.Dtos.UpdateDto;

namespace Movies.Services
{
    public interface IGenresService
    {
        Task<Response<IEnumerable<Genre>>> GetGenresAsync();
        Task<Response<Genre>> AddAsync(String genreType);
        Task<Response<Genre>> GetById(byte id);
        Task<Response<Genre>> UpdateAsync(GenreDtoUpdate dto);
        Task<Response<Genre>> DeleteAsync(byte id);

    }
}
