using System.Threading.Tasks;

namespace AppCore.Interfaces
{
    public interface IDataRepository<T>
        where T : class
    {
        void Add(T entity);

        void Delete(T entity);

        Task<T> GetByIdAsync(object id);

        Task<bool> SaveAllAsync();
    }
}
