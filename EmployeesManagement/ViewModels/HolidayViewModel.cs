using EmployeesManagement.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.ViewModels
{
    public class HolidayViewModel : UserActivity
    {
        public int Id { get; set; }

        [DisplayName("Holiday Name")]
        public string Title { get; set; }

        [DisplayName("Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy/MM/dd}")]
        public DateTime? StartDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy/MM/dd}")]
        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy/MM/dd}")]
        public Holiday Holiday { get; set; }
        public List<Holiday> Holidays { get; set; }

    }
}
