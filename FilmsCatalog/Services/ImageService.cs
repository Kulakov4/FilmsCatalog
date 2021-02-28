using FilmsCatalog.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public class ImageService<T> where T : IImageProfile, new()
    {
        private readonly IImageProfile imageProfile = new T();

        private void ValidateExtension(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);

            if (imageProfile.AllowedExtensions.Any(ext => ext == fileExtension.ToLower()))
                return;

            throw new Exception(imageProfile.BadExtensionError);
        }

        private void ValidateFileSize(IFormFile file)
        {
            if (file.Length > imageProfile.MaxSizeBytes)
                throw new Exception("Выбранный файл слишком большой");
        }

        public void ValidateImage(IFormFile file)
        {
            ValidateExtension(file);
            ValidateFileSize(file);
        }
    }
}
