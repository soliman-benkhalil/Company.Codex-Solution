﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Codex.DAL.Models
{
    public class Department : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee>? Employees { get; set; }
    }
}
