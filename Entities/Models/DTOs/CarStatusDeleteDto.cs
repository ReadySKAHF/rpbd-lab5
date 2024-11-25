using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class CarStatusDeleteDto
    {
        [Required(ErrorMessage = "Settlement Id is required.")]
        public Guid Id { get; set; }
    }
}
