namespace Movies.Services
{
    public interface IImageService
    {
        Task<Response<ImageInfo>> GetById(Guid id);
        Task<Response<ImageInfo>> SaveImageAsync(IFormFile file);
        Task<Response<Byte[]>> GetImage(Guid imageGuid);
        Task<Response<ImageInfo>> DeleteImage(Guid imageGuid);
    }
}
