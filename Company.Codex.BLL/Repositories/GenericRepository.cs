using Company.Codex.BLL.Interfaces;
using Company.Codex.DAL.Data.Contexts;
using Company.Codex.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Codex.BLL.Repositories
{
    // Here The T Can Be Any Form Of DataType Like Interface , enum,struct ,and class so where T : BaseEntity solves it
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private protected readonly AppDbContext _context;
        public GenericRepository(AppDbContext context) 
        { 
            _context = context;
        }
            

        // Async Return - void - Task - Task<>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E => E.WorkFor).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _context.Set<T>().ToListAsync();
            }
            // set is generci and returns Dbset .. so path the T And Returns The Set 
        }
        public async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<int> AddAsync(T entity)
        {
            //_context.Set<T>().Add(entity); // we can make shortcut because the object here mark the class
            await _context.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(T entity)
        {
            _context.Update(entity); // Does Not Have An Async Version
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(T entity)
        {
            _context.Remove(entity); // Does Not Have An Async Version
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetByNameAsync<T>(string name)
        {

            if (typeof(T) == typeof(Employee))
            {
                return await _context.Set<Employee>().Where(E => E.Name.ToLower().Contains(name.ToLower())).Include(E => E.WorkFor).Cast<T>().ToListAsync();
            }
            else if (typeof(T) == typeof(Department))
            {
                return await _context.Set<Department>().Where(D => D.Name.ToLower().Contains(name.ToLower())).Include(D=>D.Employees).Cast<T>().ToListAsync();
            }
            else
            {
                throw new InvalidOperationException("LoL Unsupported type");
            }
        }
    }
}
