using Movies.Dtos.ShowData;
using Movies.Dtos.UpdateDto;


namespace Movies.Services
{
    public interface IMoviesService
    {
        Task<Response<List<Movie>>> GetAllMovie();
        Task<Response<Movie>> AddMovieAsync(MovieDto dto);
        Task<Response<Movie>> GetById(int id);
        Task<Response<Movie>> UpdateMovie(MovieDtoUpdate dto);
        Task<Response<Movie>> DeleteMovie(int id);
        Task<Response<MovieDtoShow>> ShowMovieData(Movie movie);
    }
}
