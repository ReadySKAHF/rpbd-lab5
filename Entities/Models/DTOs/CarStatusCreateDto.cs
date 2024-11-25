using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class CarStatusCreateDto
    {
        [Required(ErrorMessage = "Status Name is required.")]
        public string StatusName { get; set; }
    }
}
