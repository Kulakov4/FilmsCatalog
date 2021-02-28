using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.ViewModels.Film
{
    public class FilmInfoViewModel: FilmViewModel
    {
        [Display(Name = "Постер")]
        public byte[] Poster { get; set; }    

        public int Page { get; set; }

        public bool ReadOnly { get; set; }
    }
}
