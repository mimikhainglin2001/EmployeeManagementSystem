using EmployeesManagement.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [DisplayName("Employee No")]
        public string EmpNo { get; set; }

        [DisplayName("First Name"), Required]
        public string FirstName { get; set; }

        [DisplayName("Middle Name"), Required]
        public string MiddleName { get; set; }

        [DisplayName("Last Name"), Required]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}";
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        [DisplayName("Country Name"), Required]
        public int? CountryId { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        [DisplayName("Department Name")]
        public int? DepartmentId { get; set; }

        [DisplayName("Designation Name")]
        public int? DesignationId { get; set; }

        [DisplayName("Gender Name")]
        public int? GenderId { get; set; }

        [DisplayName("Employee Photo")]
        public string? Photo { get; set; }

        [DisplayName("Employment Date"), Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy/MM/dd}")]
        public DateTime? EmploymentDate { get; set; }

        [DisplayName("Status")]
        public int? StatusId { get; set; }

        [DisplayName("Inactive Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy/MM/dd}")]
        public DateTime? InactiveDate { get; set; }
        public int? CauseofInactivityId { get; set; }

        [DisplayName("Termination Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy/MM/dd}")]
        public DateTime? TerminationDate { get; set; }
        public int? ReasonforterminationId { get; set; }

        [DisplayName("Bank Name"), Required]
        public int? BankId { get; set; }

        [DisplayName("Bank Account Number"), Required]
        public string? BankAccountNumber { get; set; }

        [DisplayName("International Bank Account No"), Required]
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

        [DisplayName("Allocated Leave Balance")]
        public Decimal? AllocatedLeaveDays { get; set; }

        [DisplayName("Leave Outstanding Balance")]
        public Decimal? LeaveOustandingBalance { get; set; }

        [DisplayName("Pay Tax")]
        public bool? PaysTax { get; set; }

        [DisplayName("Disability Type")]
        public int? DisabilityId { get; set; }

        [DisplayName("Disability Certificate")]
        public string? DisabilityCertificate { get; set; }

        [DisplayName("Employee Name")]
        public int EmployeeId {get; set; }
        
        [DisplayName("Document Name")]
        public string DocumentName { get; set; }
        
        [DisplayName("File Path")]
        public string FilePath { get; set; }
       
        [DisplayName("Document Type")]     
        public int DocumentTypeId { get; set; }
        
        [DisplayName("File Extension")]        
        public string FileExtension { get; set; }
        
        [DisplayName("File Size")]        
        public long FileSize { get; set; }
        
        [DisplayName("File Type")]      
        public string FileType { get; set; }
       
        [DisplayName("Uploaded On")]
        public DateTime UploadDate { get; set; }

        [DisplayName("Attachment")]      
        public string Attachment { get; set; }
        
        [DisplayName("Expiry On")]
        public DateTime? ExpiryDate { get; set; }
        public List<Employee> Employees { get; set; }

        public EmployeeDocument Document { get; set; }
        public List<EmployeeDocument> EmployeeDocuments { get; set; }

        public EmployeeNextOfKin NextOfKin { get; set; }
        public List<EmployeeNextOfKin> EmployeeNextOfKins { get; set; }

        public EmployeeContract Contract { get; set; }
        public List<EmployeeContract> EmployeeContracts { get; set; }

        public EmployeeHistory History { get; set; }
        public List<EmployeeHistory> EmployeeHistories { get; set; }
    }
}
