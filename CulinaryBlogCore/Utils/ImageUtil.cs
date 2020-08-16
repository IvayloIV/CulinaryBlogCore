using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace CulinaryBlogCore.Utils
{
    public static class ImageUtil
    {
        public static async Task<string> UploadImage(IFormFile image, string destination) {
            string fileName = String.Empty;
            if (image != null && image.Length > 0)
            {
                fileName = Path.GetFileName(image.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{destination}", fileName);
                using (FileStream fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileSteam);
                }
            }

            return $"/images/{destination}/{fileName}";
        }
    }
}
