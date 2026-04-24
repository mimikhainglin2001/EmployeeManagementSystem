using System.ComponentModel;

namespace EmployeesManagement.Models
{
    public class FixedAsset :UserActivity
    {
        public int Id { get; set; }

        [DisplayName("Asset No")]
        public string AssetNo { get; set; }

        [DisplayName("Asset Description")]
        public string Description { get; set; }

        [DisplayName("Asset Category")]
        public int? CategoryId { get; set; }
        public SystemCodeDetail Category {  get; set; }

        [DisplayName("Asset Serial No")]
        public string SerialNo { get; set; }

        [DisplayName("Asset Model")]
        public string Model { get; set; }

        [DisplayName("Asset Status")]
        public int StatusId { get; set; }
        public SystemCodeDetail Status { get; set; }

        [DisplayName("Employee Name")]
        public int ResponsibleEmployeeId { get; set; }
        public Employee ResponsibleEmployee { get; set; }
        public int? LocationId { get; set; }
        public SystemCodeDetail Location { get; set; }

        [DisplayName("Asset Photo")]
        public string? Photo { get; set; }

        [DisplayName("Asset Notes")]
        public string? Notes { get; set; }

        [DisplayName("Purchase Date")]
        public DateTime? PurchaseDate { get; set; }   
    }
}
