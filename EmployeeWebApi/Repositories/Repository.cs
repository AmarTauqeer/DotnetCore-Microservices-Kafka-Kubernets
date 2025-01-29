
using EmployeeWebApi.Data;
using EmployeeWebApi.Repositories.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeWebApi.Interfaces
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly EmployeeDB _departmentDbContext;
        public Repository(EmployeeDB departmentDbContext)
        {
            _departmentDbContext = departmentDbContext;
        }
        public async Task<T> CreateAsync(T entity)
        {
            await _departmentDbContext.Set<T>().AddAsync(entity);
            await _departmentDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _departmentDbContext.Set<T>().Remove(entity);
            int row = await _departmentDbContext.SaveChangesAsync();
            return row > 0;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _departmentDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _departmentDbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
           _departmentDbContext.Set<T>().Update(entity);
            await _departmentDbContext.SaveChangesAsync();
            return entity;
        }
    }
}
