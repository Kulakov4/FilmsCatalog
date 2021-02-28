using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.ViewModels.Film
{
    public class FilmListViewModel
    {
        public IEnumerable<FilmsCatalog.Models.Film> Films { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
