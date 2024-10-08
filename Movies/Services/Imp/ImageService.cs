
using Microsoft.EntityFrameworkCore;
using Movies.Model;
using static System.Net.Mime.MediaTypeNames;

namespace Movies.Services.Imp
{
    public class ImageService : IImageService
    {
        private readonly ApplictionDbContext _context;

        private readonly List<string> _allowedExtenstions = new() { ".jpg", ".png" };
        private readonly long _maxAllowedPosterSize = 1024 * 1024 * 5;
        private readonly string _folderPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Images");

        public ImageService(ApplictionDbContext context)
        {
            _context = context;
        }

        public async Task<Response<ImageInfo>> DeleteImage(Guid imageGuid)
        {
            
            try
            {
                var imageRecord = await GetById(imageGuid);
                if (!imageRecord.IsSuccess)
                    return imageRecord;
                

                // Remove image from database
                _context.Images.Remove(imageRecord.Value);
                await _context.SaveChangesAsync();

                // Remove image from folder
                if (File.Exists(imageRecord.Value.Path))
                {
                    File.Delete(imageRecord.Value.Path);
                }
                return imageRecord;
            }
            catch (FileNotFoundException ex)
            {
                return new Response<ImageInfo>
                {
                    ErrorMessage = ex.Message,
                    IsSuccess = false
                };
            }
            catch (Exception ex)
            {
                return new Response<ImageInfo>
                {
                    ErrorMessage = ex.Message,
                    IsSuccess = false
                };
            }
        }
    

        public async Task<Response<ImageInfo>> GetById(Guid id)
        {
            var res = new Response<ImageInfo>();
            try
            {
                res.Value = await _context.Images.SingleOrDefaultAsync(g => g.Id == id);
                res.IsSuccess = true;
                if (res.Value == null)
                {
                    res.IsSuccess = false;
                    res.ErrorMessage = "Image didn't found.";
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
        public async Task<Response<Byte[]>> GetImage(Guid imageGuid)
        {
            var res = new Response<Byte[]>();
            
                var imageInfo = await GetById(imageGuid);
            if (!imageInfo.IsSuccess)
            {
                res.ErrorMessage = imageInfo.ErrorMessage;
                res.IsSuccess = false;
                return res;
            }

            var imageType = imageInfo.Value.ImageType;
            var filePath = Path.Combine(_folderPath, 
                $"{imageGuid + imageType}"); 

            if (!File.Exists(filePath)) {
                res.ErrorMessage = "Image not found.";
                res.IsSuccess = false;
                return res;
            }
            try
            {
                var imageBytes = await File.ReadAllBytesAsync(filePath);
                res.Value = imageBytes;

                res.IsSuccess = true;

                return res;
            }
            catch (Exception ex) { 
                res.IsSuccess = false;
                res.ErrorMessage = ex.Message;
                return res;
            }
            
        }

        public async Task<Response<ImageInfo>> SaveImageAsync(IFormFile file)
        {
            var res = new Response<ImageInfo>();
            res.Value =new ImageInfo();
            if (_maxAllowedPosterSize < file.Length)
            {
                res.ErrorMessage = "Max size 5 mb";
                    res.IsSuccess = false;
                return res;
            }

            var imageId = Guid.NewGuid();
            
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!_allowedExtenstions.Contains(extension))
            {                
                    res.ErrorMessage = "Only .png and .jpg images are allowed";
                    res.IsSuccess = false;
                return res;
            }


            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            var fileName = $"{imageId}{extension}";
            var filePath = Path.Combine(_folderPath, fileName);


           
            try
            {
                res.Value.Path = filePath;
                res.Value.ImageType = extension;
                res.Value.Id = imageId;

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                _context.Add(res.Value);
                await _context.SaveChangesAsync();

                res.IsSuccess = true;
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
