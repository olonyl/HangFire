using HangFire.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HangFire.Infrastructure.Repositories
{
    public class EfRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly ApplicationContext _dbContext;

        public EfRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> AddAsync(List<T> entity)
        {
            foreach (var item in entity)
            {
                _dbContext.Set<T>().Add(item);
            }

            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> DeleteAsync(List<T> entity)
        {
            foreach (var item in entity)
            {
                _dbContext.Set<T>().Remove(item);
            }

            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> UpdateAsync(List<T> entity)
        {
            foreach (var item in entity)
            {
                _dbContext.Entry(item).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

    }

}
