using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.Models
{
    public class Employee: UserActivity
    {
        public int Id { get; set; }
        public string EmpNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}";
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
       
        [DisplayName("Country Name")]
        public int? CountryId { get; set; }
        public Country Country { get; set; }

        [DisplayName("Date of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy/MM/dd}")]
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        
        [DisplayName("Department Name")]
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        
        [DisplayName("Designation Name")]
        public int? DesignationId { get; set; }
        public Designation Designation { get; set; }
        
        [DisplayName("Gender Name")]
        public int? GenderId { get; set; }
        public SystemCodeDetail Gender { get; set; }
        
        [DisplayName("Employee Photo")]
        public string? Photo { get; set; }

        [DisplayName("Employment Date")]
        [DisplayFormat(ApplyFormatInEditMode  = true, DataFormatString = "{0:yyy/MM/dd}")]
        public DateTime? EmploymentDate { get; set; }
        public int? StatusId { get; set; }
        public SystemCodeDetail Status { get; set; }

        [DisplayName("Inactive Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy/MM/dd}")]
        public DateTime? InactiveDate { get; set; }
        public int? CauseofInactivityId { get; set; }
        public SystemCodeDetail CauseofInactivity { get; set; }

        [DisplayName("Termination Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy/MM/dd}")]
        public DateTime? TerminationDate { get; set; }
        public int? ReasonforterminationId { get; set; }
        public SystemCodeDetail Reasonfortermination { get; set; }
        
        [DisplayName("Bank Name")]
        public int? BankId { get; set; }
        public Bank Bank { get; set; }
        
        [DisplayName("Bank Account Number")]
        public string? BankAccountNumber { get; set; }
        
        [DisplayName("International Bank Account No")]
        public string? IBAN { get; set; }
        
        [DisplayName("SWIFT Code")]
        public string? SWIFTCode { get; set; }
        
        [DisplayName("N.S.S.F Number")]
        public string? NSSFNO { get; set; }
       
        [DisplayName("NHIF Number")]
        public string? NHIF { get; set; }
        
        [DisplayName("Company Email Address")]
        public string? CompanyEmail { get; set; }
        
        [DisplayName("KRA PIN")]
        public string? KRAPIN { get; set; }

        [DisplayName("Passport No")]
        public string? PassportNo { get; set; }

        [DisplayName("Employment Terms")]
        public int? EmploymentTermsId { get; set; }
        public SystemCodeDetail EmploymentTerms { get; set; }

        [DisplayName("Allocated Leave Balance")]
        public double? AllocatedLeaveDays { get; set; }
        
        [DisplayName("Leave Outstanding Balance")]
        public double? LeaveOustandingBalance { get; set; }

        [DisplayName("Pay Tax")]
        public bool? PaysTax { get; set; }

        [DisplayName("Disability Type")]
        public int? DisabilityId { get; set; }
        public SystemCodeDetail Disability { get; set; }
        public string? DisabilityCertificate { get; set; }
    }
}
