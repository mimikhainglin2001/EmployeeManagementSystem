using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.Models
{
    public class EmployeeContract: UserActivity
    {
        public int Id { get; set; }
        public int EmployeeId {get; set; }
        public Employee Employee { get; set; }
        public string ContractTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ContractTypeId { get; set; }
        public SystemCodeDetail ContractType { get; set; }
        public string ContractFilePath { get; set; }
        public int? ProbationperiodMonths { get; set; }
        public decimal Salary { get; set; }
        public int CurrencyTypeId { get; set; }
        public SystemCodeDetail CurrencyType { get; set; }
        public string TermsAndConditions { get; set; }
        public bool IsActive { get; set; }
    }
}
