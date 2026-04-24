using System.ComponentModel;

namespace EmployeesManagement.Models
{
    public class ApprovalEntry
    {
        public int Id { get; set; }

        [DisplayName("Record Id")]
        public int RecordId { get; set; } //1

        [DisplayName("Document Type")]
        public int DocumentTypeId { get; set; } // Leave application
        public SystemCodeDetail DocumentType { get; set; }

        [DisplayName("Sequence No")]
        public int SequenceNo { get; set; } // 1,2,3,4,5 (Approvals)

        [DisplayName("Approver Name")]
        public string ApproverId { get; set; } // 1,2,3,4,5 (Approvers)
        public ApplicationUser Approver { get; set; } // 1,2,3,4,5 (Approvers)

        [DisplayName("Status")]
        public int StatusId { get; set; } // Status of Document
        public SystemCodeDetail Status { get; set; }

        [DisplayName("Date Sent for Approval")]
        public DateTime DateSentForApproval { get; set; } // Date sent for approval

        [DisplayName("Last Modified On")]
        public DateTime LastModifiedOn { get; set; } // The action of the approvers

        [DisplayName("Last Modified By")]
        public string LastModifiedById { get; set; } 
        public ApplicationUser LastModifiedBy { get; set; }

        [DisplayName("Comments")]
        public string Comments { get; set; }
        
        [DisplayName("Controller Name")]
        public string ControllerName { get; set; }
    }
}
