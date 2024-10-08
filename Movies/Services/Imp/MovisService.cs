using Microsoft.EntityFrameworkCore;
using Movies.Dtos;
using Movies.Dtos.ShowData;
using Movies.Dtos.UpdateDto;
using Movies.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Movies.Services.Imp
{
    public class MovisService : IMoviesService
    {
        private readonly ApplictionDbContext _context;
        private readonly IImageService _imageService;
        private readonly IGenresService _genresService;

        public MovisService(ApplictionDbContext context, IImageService imageService, IGenresService genresService)
        {
            _context = context;
            _imageService = imageService;
            _genresService = genresService;
        }

        public async Task<Response<Movie>> AddMovieAsync(MovieDto dto)
        {
            var res=new Response<Movie>();
            try
            {

                
                var movie = new Movie
                {
                    GenreId = dto.GenreId,
                    Title = dto.Title,
                    Rate = dto.Rate,
                    StoreLine = dto.StoreLine,
                    Year = dto.Year
                };
                if (dto.Poster != null) {

                    var imageInfo = await _imageService.SaveImageAsync(dto.Poster);
                    if (!imageInfo.IsSuccess)
                    {
                        res.IsSuccess = false;
                        res.ErrorMessage = imageInfo.ErrorMessage;

                        return res;
                    }
                    movie.ImageGuid = imageInfo.Value.Id;
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();

                res.Value = movie;
                res.IsSuccess = true;
                return res;
            }
            catch (Exception ex) { 
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
                return res;
            }
        }

        public async Task<Response<Movie>> DeleteMovie(int id)
        {
            var res = await GetById(id);
            if (!res.IsSuccess)
                return res;
            try
            {
                if (res.Value.ImageGuid != null)
                {
                    var delImage = await _imageService.DeleteImage(res.Value.ImageGuid ?? new Guid());
                    if (!delImage.IsSuccess)
                    {
                        res.IsSuccess = false;
                        res.ErrorMessage = "try Again" + delImage.ErrorMessage;
                        return res;
                    }
                }
                _context.Remove(res.Value);
                await _context.SaveChangesAsync();
                return res;
            }
            catch (Exception ex) { 
                res.ErrorMessage = ex.Message;
                res.IsSuccess = false;

                return res;
            }
        }

        public async Task<Response<List<Movie>>> GetAllMovie()
        {
            var res = new Response<List<Movie>>();
            try
            {
                var movies = await _context.Movies.Include(m => m.Genre).ToListAsync();
                res.IsSuccess =true;
                res.Value = movies;

                return res;
            //    var movieDtos = new List<MovieDtoShow>();

            //    // Iterate over each movie and map to MovieDto
            //    foreach (var movie in movies)
            //    {
            //        var movieDto = new MovieDtoShow
            //        {
            //            Id = movie.Id,
            //            Title = movie.Title,
            //            Year = movie.Year,
            //            Rate = movie.Rate,
            //            StoreLine = movie.StoreLine,
            //            GenreName = movie.Genre.Name
            //        };

            //        // Fetch the image using GetImage method if the ImageGuid is present
            //        if (movie.ImageGuid.HasValue)
            //        {
            //            var imageResponse = await _imageService.GetImage(movie.ImageGuid.Value);
            //            if (imageResponse.IsSuccess)
            //            {
            //                movieDto.ImageData = imageResponse.Value;
            //            }
            //            else
            //            {
            //                movieDto.ImageData = null;  // Handle case where image isn't found
            //            }
            //        }

            //        // Add the movieDto to the list
            //        movieDtos.Add(movieDto);
            //    }

            //    res.IsSuccess = true;
            //    res.Value = movieDtos;
            //    return res;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
                res.IsSuccess = false;

                return res;
            }
        }

        public async Task<Response<Movie>> GetById(int id)
        {
            var res=new Response<Movie>();
            try
            {
                res.Value = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
                if (res.Value == null)
                {
                    res.ErrorMessage = "Movie not found";
                    res.IsSuccess = false;

                    return res;
                }
                res.IsSuccess=true;
                return res;
            }
            catch (Exception ex) { 
                res.ErrorMessage = ex.Message;
                res.IsSuccess = false;

                return res;
            }
        }

        public async Task<Response<MovieDtoShow>> ShowMovieData(Movie movie)
        {
            var res = new Response<MovieDtoShow>
            {
                IsSuccess = true,
            };
            try
            {

                var movieDto = new MovieDtoShow
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Year = movie.Year,
                    Rate = movie.Rate,
                    StoreLine = movie.StoreLine,
                };
                var genreName = await _genresService.GetById(movie.GenreId);
                if(genreName.IsSuccess)
                    movieDto.GenreName = genreName.Value.Name;

                // Fetch the image using GetImage method if the ImageGuid is present
                if (movie.ImageGuid.HasValue)
                {
                    var imageResponse = await _imageService.GetImage(movie.ImageGuid.Value);
                    if (imageResponse.IsSuccess)
                    {
                        movieDto.ImageData = imageResponse.Value;
                    }
                    else
                    {
                        res.IsSuccess = false;
                    }
                }
                res.Value = movieDto;
                return res;
            }
            catch (Exception ex) { 
                res.ErrorMessage=ex.Message +"Action done but can't show movie data now";
                res.IsSuccess=false;
                return res;
            }
        }

        public async Task<Response<Movie>> UpdateMovie(MovieDtoUpdate dto)
        {
            var res = await GetById(dto.Id);
            if (!res.IsSuccess)
                return res;
            try
            {
                if (dto.GenreId > 0)
                    res.Value.GenreId = dto.GenreId;
                if (dto.Title != null)
                    res.Value.Title = dto.Title;
                if (dto.Rate > -1)
                    res.Value.Rate = dto.Rate;
                if (dto.StoreLine != null)
                    res.Value.StoreLine = dto.StoreLine;
                if (dto.Year > -1)
                    res.Value.Year = dto.Year;

                if (dto.Poster != null)
                {

                    var imageInfo = await _imageService.SaveImageAsync(dto.Poster);
                    if (!imageInfo.IsSuccess)
                    {
                        res.IsSuccess = false;
                        res.ErrorMessage = imageInfo.ErrorMessage;

                        return res;
                    }
                    res.Value.ImageGuid = imageInfo.Value.Id;
                }

                _context.Update(res.Value);
                await _context.SaveChangesAsync();

                return res;
            }
            catch (Exception ex) {
                res.ErrorMessage = ex.Message;
                res.IsSuccess = false;

                return res;
            }
        }
    }
}
