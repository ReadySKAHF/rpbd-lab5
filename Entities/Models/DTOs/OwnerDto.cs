using Entities.Base;

namespace Entities.Models.DTOs
{
    public class OwnerDto : EntityBaseDto
    {
        public string DriverLicenseNumber { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

    }
}
