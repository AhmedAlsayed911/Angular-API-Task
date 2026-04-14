namespace Madrasty_API.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T data);
        Task<T> UpdateAsync(T data);
        void DeleteAsync(int id);
    }
}
