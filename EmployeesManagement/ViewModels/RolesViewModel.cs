using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.ViewModels
{
    public class RolesViewModel
    {
        public string? RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }
    }
}