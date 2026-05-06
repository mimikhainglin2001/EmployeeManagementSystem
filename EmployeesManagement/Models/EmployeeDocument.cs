using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.Models
{
    public class EmployeeDocument: UserActivity
    {
        public int Id { get; set; }

        [DisplayName("Employee Name")]
        public int EmployeeId {get; set; }
        public Employee Employee { get; set; }
        
        [DisplayName("Document Name")]
        public string DocumentName { get; set; }
        
        [DisplayName("File Path")]
        public string FilePath { get; set; }
       
        [DisplayName("Document Type")]     
        public int DocumentTypeId { get; set; }
        public SystemCodeDetail DocumentType { get; set; }
        
        [DisplayName("File Extension")]        
        public string FileExtension { get; set; }
        
        [DisplayName("File Size")]        
        public long FileSize { get; set; }
        
        [DisplayName("File Type")]      
        public string FileType { get; set; }
       
        [DisplayName("Uploaded On")]
        public DateTime UploadDate { get; set; }
        
        [DisplayName("Expiry On")]
        public DateTime? ExpiryDate { get; set; }
    }
}
