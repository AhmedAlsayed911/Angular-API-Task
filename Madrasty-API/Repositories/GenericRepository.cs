using Madrasty.Entites;
using Microsoft.EntityFrameworkCore;

namespace Madrasty_API.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext dbContext)
        : IGenericRepository<T> where T : class
    {
        public async Task<List<T>> GetAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

    public async Task<T> AddAsync(T data)
    {
        await dbContext.Set<T>().AddAsync(data);
        return data;
    }

        public async Task<T> UpdateAsync(T data)
        {
            dbContext.Set<T>().Update(data);
            return data;
        }

        public void DeleteAsync(int id)
        {
            dbContext.Set<T>().Remove(dbContext.Set<T>().Find(id));
        }

    }
}
