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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository // No Error Here Because The Interface Is Satisifed Because it Sees The Implementaions in GenericRepository 
    {

        public EmployeeRepository(AppDbContext context) : base(context)// Ask CLR To Create Object From AppDbContext
        {
        }



    }
}
