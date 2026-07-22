using Microsoft.AspNetCore.Http; 
namespace AppServices
{
    public interface IFileService
    {
        Task<string?> SaveFileAsync(IFormFile? file, string uploadPath, string prefix = "", bool saveToDisk = true);
    }

    public class FileService : IFileService
    {
        public FileService() { }

        public async Task<string?> SaveFileAsync(IFormFile? file, string uploadPath, string prefix = "", bool saveToDisk = true)
        {
            if (file == null) return string.Empty;
            var fileName = "";
            if (!saveToDisk) {  
                var extension = Path.GetExtension(file.FileName);
                fileName = $"{prefix}{DateTime.Now:yyyyMMdd_HHmmss_fff}{extension}";
            } 
            if (saveToDisk)
            {
                fileName = prefix;
                var fullFilePath = Path.Combine(uploadPath, fileName);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return fileName; // Just return file name
        }
    }

}
