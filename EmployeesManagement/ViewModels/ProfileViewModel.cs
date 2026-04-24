using EmployeesManagement.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.ViewModels
{
    public class ProfileViewModel
    {
        public ICollection<SystemProfile> Profiles { get; set; } //list of all tasks to show in a dropdown
        public ICollection<int> RolesRightsIds { get; set; }
        public int[] Ids {  get; set; }

        [DisplayName("Role")]
        public string RoleId { get; set; }
        [DisplayName("System Task")]
        public int TaskId { get; set; }


    }
}
