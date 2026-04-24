using System.ComponentModel;

namespace EmployeesManagement.Models
{
    public class LeaveType:UserActivity
    {
        public int Id { get; set; }

        [DisplayName("Leave Type Code")]
        public string Code { get; set; }

        [DisplayName("Leave Type Name")]
        public string Name { get; set; }

        [DisplayName("Leave Type Days")]
        public decimal Days { get; set; }
    }
}
