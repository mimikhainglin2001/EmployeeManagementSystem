using System.ComponentModel;

namespace EmployeesManagement.Models
{
    public class Audit
    {
        public int Id { get; set; }

        [DisplayName("User Name")]
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }

        [DisplayName("Audit Type")]
        public string AuditType { get; set; }

        [DisplayName("Table Name")]
        public string TableName { get; set; }

        [DisplayName("Timestamp")]
        public DateTime DateTime { get; set; }

        [DisplayName("Old Values")]
        public string? OldValues { get; set; }

        [DisplayName("New Values")]
        public string? NewValues { get; set; }

        [DisplayName("Affected Columns")]
        public string? AffectedColumns { get; set; }

        [DisplayName("Primary Key")]
        public string PrimaryKey { get; set; }
    }

    public enum AuditType
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3,
        Login = 4,

    }
}
