using Company.Codex.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Codex.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {

        Task<IEnumerable<T>> GetByNameAsync<T>(string name);

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
    }
}
