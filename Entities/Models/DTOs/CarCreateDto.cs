using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class CarCreateDto
    {
        [Required(ErrorMessage = "LicensePlate is required.")]
        public string LicensePlate { get; set; } = null!;

        [Required(ErrorMessage = "Brand is required.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Power is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Cargo Weight must be a positive number.")]
        public int Power { get; set; }

        [Required(ErrorMessage = "Color is required.")]
        public string Color { get; set; }

        [Required(ErrorMessage = "YearOfProduction is required.")]
        public int YearOfProduction { get; set; }

        [Required(ErrorMessage = "ChassisNumber is required.")]
        public string ChassisNumber { get; set; }

        [Required(ErrorMessage = "EngineNumber is required.")]
        public string EngineNumber { get; set; }

        [Required(ErrorMessage = "DateReceived is required.")]
        public DateTime DateReceived { get; set; }

        [Required(ErrorMessage = "OwnerId is required.")]
        public Guid OwnerId { get; set; }
    }
}
