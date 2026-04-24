using System.ComponentModel;

namespace EmployeesManagement.Models
{
    public class Holiday: UserActivity
    {
        [DisplayName("No")]
        public int Id { get; set; }

        [DisplayName("Holiday Name")]
        public string Title { get; set; }


        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

    }
}
