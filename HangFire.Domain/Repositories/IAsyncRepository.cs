using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HangFire.Domain.Repositories
{
    public interface  IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<List<T>> AddAsync(List<T> entity);
        Task<List<T>> DeleteAsync(List<T> entity);
        Task<List<T>> UpdateAsync(List<T> entity);
    }
}
