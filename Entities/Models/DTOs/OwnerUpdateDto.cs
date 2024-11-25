using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class OwnerUpdateDto : EntityBaseDto
    {

        [Required(ErrorMessage = "DriverLicenseNumber is required.")]
        public string DriverLicenseNumber { get; set; }
        [Required(ErrorMessage = "FullName is required.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Adress is required.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        public string? Phone { get; set; }

    }
}
