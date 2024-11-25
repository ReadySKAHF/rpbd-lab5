using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class CarDeleteDto
    {
        [Required(ErrorMessage = "Car Id is required.")]
        public Guid Id { get; set; }
    }
}
