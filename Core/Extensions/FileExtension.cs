namespace Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class FileExtension
	{
        #region Params
        private static readonly Dictionary<string, string> FileSignatureMappings = new()
        {
            { "FFD8FFE0", ".jpg" },
            { "89504E47", ".png" },
            { "25504446", ".pdf" }
        };
        #endregion

        public static string GetUniqueCode(this string IdCode)
        {
            string CodigoUnico = DateTime.Now.ToString("yyyyMMddHHmmss_fff") + "_" + IdCode + "_" + Path.GetRandomFileName().PadLeft(11).Replace('.', '_');
            return CodigoUnico;
        }

        public static byte[] Base64ToBytes(this string base64)
        {
            return base64 != null ? Convert.FromBase64String(base64) : null;
        }


        public static string GetFileExtension(this byte[] bytes)
        {
            string fileSignature = GetHexSignature(bytes, 8);
            string truncatedFileSignature = fileSignature.Substring(0, Math.Min(fileSignature.Length, 8));
            if (FileSignatureMappings.TryGetValue(truncatedFileSignature, out string extension)) return extension;
            return ".bin";
        }

        public static string GetMimeType(this string fileExtension)
        {
            switch (fileExtension)
            {
                case ".jpg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream";
            }
        }

        public static string UNCPath(this string path)
        {
            if (!path.StartsWith(@"\\")) path = @"\\" + path.TrimStart('\\');
            return path;
        }

        #region Private Methods
        private static string GetHexSignature(byte[] bytes, int length)
        {
            if (bytes.Length < length) return string.Empty;
            return BitConverter.ToString(bytes, 0, length).Replace("-", "");
        }
        #endregion
    }
}

