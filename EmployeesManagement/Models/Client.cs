using System.ComponentModel;

namespace EmployeesManagement.Models
{
    public class Client : UserActivity
    {
        public int Id { get; set; }

        [DisplayName("Client Code")]
        public string Code { get; set; }
        [DisplayName("Client Name")]
        public string Name { get; set; }

        [DisplayName("Client Address")]
        public string Address { get; set; }

        [DisplayName("Client Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Client Email")]
        public string Email { get; set; }

        [DisplayName("Client Status")]
        public int StatusId { get; set; }
        public SystemCodeDetail Status {  get; set; }

    }
}
