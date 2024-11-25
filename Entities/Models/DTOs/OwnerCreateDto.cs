using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class OwnerCreateDto
    {
        [Required(ErrorMessage = "DriverLicenseNumber is required.")]
        public string DriverLicenseNumber { get; set; } = null!;
        [Required(ErrorMessage = "FullName is required.")]
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage = "Adress is required.")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        public string? Phone { get; set; }

    }
}
