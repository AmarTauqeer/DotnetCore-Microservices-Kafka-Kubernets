namespace EmployeeWebApi.Repositories.Common
{

   public interface IRepository<T> where T : class
   {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
   }
}
