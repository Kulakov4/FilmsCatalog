using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.ViewModels.Film
{
    public class EditFilmViewModel: FilmViewModel
    {
        public byte[] Poster { get; set; }      // Постер

        [Display(Name = "Постер")]
        public IFormFile PosterFile { get; set; }
    }
}
