using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeesManagement.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        

        [Required(ErrorMessage = "UserName is required")]
        [DisplayName("User Name")]

        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [DisplayName("Phone Number")]

        public string PhoneNumber { get; set; }

        [DisplayName("Address")]

        public string? Address { get; set; }

        [DisplayName("First Name")]

        public string? FirstName { get; set; }
        [DisplayName("Middle Name")]

        public string? MiddleName { get; set; }
       
        [DisplayName("Last Name")]
        public string? LastName { get; set; }
       
        [DisplayName("National Id")]
        public string? NationalId { get; set; }

        public string? FullName => $"{FirstName} {MiddleName} {LastName}";
       
        [Required(ErrorMessage = "Role is required")]
       
        [DisplayName("User Role")]

        public string RoleId { get; set; }


    }
}