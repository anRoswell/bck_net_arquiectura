namespace Core.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Core.DTOs.FilesDto;
    using Core.Extensions;
    using Core.Interfaces;

    public class FileManagementService : IFileManagementService
    {

        public async Task<FileResponse> CreateFileByBase64(FileByBase64Dto fileByBase64Dto)
        {
            byte[] fileBytes = fileByBase64Dto.Base64.Base64ToBytes();
            string fileExtension = fileBytes.GetFileExtension();
            string uniqueCode = fileByBase64Dto.IdRoute.ToString().GetUniqueCode();
            string fileName = string.Concat(uniqueCode, fileExtension);
            string pathComplete = Path.Combine(fileByBase64Dto.PathBase, fileName);
            CreateFolder(fileByBase64Dto.PathBase);
            using (FileStream fs = new(pathComplete, FileMode.Create, FileAccess.Write))
            {
                await fs.WriteAsync(fileBytes);
            }
            FileInfo fileInfo = new(pathComplete);
            double sizeInMegabytes = (double)fileBytes.Length / (1024L * 1024L);
            FileResponse fileData = new()
            {
                Extension = fileInfo.Extension.GetMimeType(),
                NombreInterno = fileName,
                NombreOriginal = fileName,
                Size = sizeInMegabytes,
                PathWebRelative = pathComplete
            };

            return fileData;
        }

        public async Task<FileResponse> CreateFileByBase64(NewFileByBase64Dto fileByBase64Dto)
        {
            byte[] fileBytes = fileByBase64Dto.Base64.Base64ToBytes();
            string fileExtension = fileBytes.GetFileExtension();
            string uniqueCode = fileByBase64Dto.IdRoute.ToString().GetUniqueCode();
            string fileName = string.Concat(uniqueCode, fileExtension);
            string pathComplete = Path.Combine(fileByBase64Dto.PathBase, fileName);
            CreateFolder(fileByBase64Dto.PathBase);
            using (FileStream fs = new(pathComplete, FileMode.Create, FileAccess.Write))
            {
                await fs.WriteAsync(fileBytes);
            }
            FileInfo fileInfo = new(pathComplete);
            double sizeInMegabytes = (double)fileBytes.Length / (1024L * 1024L);
            FileResponse fileData = new()
            {
                Extension = fileInfo.Extension.GetMimeType(),
                NombreInterno = fileName,
                NombreOriginal = fileName,
                Size = sizeInMegabytes,
                PathWebRelative = pathComplete
            };

            return fileData;
        }

        public async Task<string> CreateBase64ByUrl(string url)
        {
            if (File.Exists(url))
            {
                byte[] fileBytes = await File.ReadAllBytesAsync(url);
                string base64 = Convert.ToBase64String(fileBytes);
                return base64;
            }
            else
            {
                return string.Empty;
            }
        }

        #region Private Methods
        public static void CreateFolder(string folder)
        {
            string currentYearMonth = DateTime.Now.ToString("yyyyMM");
            string fullPath = Path.Combine(folder, currentYearMonth);
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
        }
        #endregion
    }
}