using FilmsCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Interfaces
{
    public interface ICrudService<T> where T : BaseEntity
    {
        public Task Save();
        public Task<T> Insert(T o);
        public IQueryable<T> GetAll();
        public Task<T> Get(int id);
        public Task<T> Update(T o);
        public Task<bool> Delete(int id);

    }
}
