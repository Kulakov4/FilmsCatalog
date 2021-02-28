using FilmsCatalog.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Models
{
    public class PosterImageProfile : IImageProfile
    {
        private const int mb = 1048576;

        public PosterImageProfile()
        {
            AllowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif" };
        }

        public string Folder => "Posters";
        public int MaxSizeBytes => 10 * mb;
        public IEnumerable<string> AllowedExtensions { get; }

        public string BadExtensionError
        { 
            get 
            {

                return $"Постер должен быть изображением ({String.Join(", ", AllowedExtensions)}).";
            } 
        }
    }
}
