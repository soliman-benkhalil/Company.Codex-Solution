using Company.Codex.DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.Codex.PL.ViewModels.DepartmentViews
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Code Is Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }
        [DisplayName("Date Of Creation")]
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee>? Employees { get; set; }
    }
}
