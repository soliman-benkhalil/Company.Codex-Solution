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
    // CLR
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {

        public DepartmentRepository(AppDbContext context) : base(context) 
        {
        }
    }
}
