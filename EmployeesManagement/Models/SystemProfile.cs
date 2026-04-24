using System.ComponentModel;

namespace EmployeesManagement.Models
{
    public class SystemProfile : UserActivity
    {
        public int Id { get; set; }

        [DisplayName("Profile Name")]
        public string Name { get; set; }

        [DisplayName("Parent Profile")]
        public int? ProfileId {  get; set; }
        public SystemProfile Profile { get; set; }
        public ICollection<SystemProfile> Children { get; set; }

        [DisplayName("Order No")]
        public int? Order { get; set; }



    }
}
