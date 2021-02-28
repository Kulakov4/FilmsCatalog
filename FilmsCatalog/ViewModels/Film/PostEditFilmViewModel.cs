using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.ViewModels.Film
{
    public class PostEditFilmViewModel: FilmViewModel
    {
        public IFormFile PosterFile { get; set; }
    }
}
