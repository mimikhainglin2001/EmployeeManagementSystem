using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.Models
{
    public class EmployeeHistory: UserActivity
    {
        public int Id { get; set; }
        public int EmployeeId {get; set; }
        public Employee Employee { get; set; }
        public int? OldDepartmentId { get; set; }
        public Department OldDepartment { get; set; }
        public int? NewDepartmentId { get; set; }
        public Department NewDepartment { get; set; }
        public int? OldDesignationId { get; set; }
        public Designation OldDesignation { get; set; }
        public int? NewDesignationId { get; set; }
        public Designation NewDesignation { get; set; }
        public decimal? OldSalary { get; set; }
        public decimal? NewSalary { get; set; }
        public int ChangeTypeId { get; set; } // Promotion, Demotion, Transfer, Salary Change, etc... 
        public SystemCodeDetail ChangeType { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Reason { get; set; }
        
    }
}
