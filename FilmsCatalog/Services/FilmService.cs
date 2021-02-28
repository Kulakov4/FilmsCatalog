using FilmsCatalog.Data;
using FilmsCatalog.Interfaces;
using FilmsCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Services
{
    public class FilmService : CrudService<Film>, ICrudService<Film>
    {
        public FilmService(ApplicationDbContext DbContext) : base(DbContext) {
        }

        public async void Test()
        {
            var f = new Film();
            var result = await Insert(f);
        }
    }
}
