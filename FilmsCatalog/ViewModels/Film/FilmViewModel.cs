using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.ViewModels.Film
{
    public class FilmViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Название фильма")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Год выпуска")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Режиссёр")]
        public string Director { get; set; }

        public string ReturnUrl { get; set; }
    }
}
