using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.Models
{
    public class EmployeeNextOfKin: UserActivity
    {
        public int Id { get; set; }
        public int EmployeeId {get; set; }
        public Employee Employee { get; set; }
        public string FullName { get; set; }
        public int RelationshipId { get; set; }
        public SystemCodeDetail Relationship { get; set; }

        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Occupation { get; set; }

    }
}
