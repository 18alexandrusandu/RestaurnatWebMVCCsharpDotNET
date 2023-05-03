using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;
using WebApplication5.Models;

namespace WebApplication5.Repositories
{
    public class GenericRepository<T> where T : class
    {
        protected readonly RestaurantDbContext _context;

        public GenericRepository(RestaurantDbContext context)
        {
            _context = context;
        }

        public  async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
      

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync<T>();
        }

        public async Task Add(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
             _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }


    }

}
