using Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models.DTOs
{
    public class CarStatusUpdateDto : EntityBaseDto
    {
        [Required(ErrorMessage = "Status Name is required.")]
        public string StatusName { get; set; }
    }
}
