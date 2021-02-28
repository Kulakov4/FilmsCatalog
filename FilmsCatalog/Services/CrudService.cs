using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FilmsCatalog.Data;
using FilmsCatalog.Extensions;
using FilmsCatalog.Interfaces;
using FilmsCatalog.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FilmsCatalog.Services
{
    public class CrudService<T> where T : BaseEntity, new()
    {
        protected ApplicationDbContext DbContext { get; set; }

        public CrudService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task Save()
        {
            try
            {
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка при сохранении в БД" + ex.Message);
            }
        }

        public async Task<T> Insert(T o)
        {
            var t = new T();
            t.CopyProperties(o);
            await DbContext.Set<T>().AddAsync(t);
            return t;
        }

        public virtual async Task<T> Update(T o)
        {
            if (o.Id == 0)
            {
                throw new Exception("Нельзя обновить объект с идентификатором 0");
            }

            T oldItem;
            oldItem = await Get(o.Id);
            oldItem.CopyProperties(o);
            await Save();
            o.CopyProperties(oldItem);
            return o;
        }

        public virtual void Delete(T o)
        {
            try
            {
                DbContext.Set<T>().Remove(o);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public virtual async Task<bool> Delete(int id)
        {
            try
            {
                var o = await Get(id);
                if (o == null)
                    return false;
                Delete(o);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate);
        }

        public async Task<T> Get(int id)
        {
            return await DbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public virtual IQueryable<T> GetAll()
        {
            return DbContext.Set<T>();
        }
    }
}
