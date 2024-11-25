using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class OwnerDeleteDto
    {
        [Required(ErrorMessage = "Owner ID is required.")]
        public Guid Id { get; set; }
    }
}
