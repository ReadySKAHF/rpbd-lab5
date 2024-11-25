using Entities.Base;

namespace Entities
{
    public class Owner : EntityBase
    {
        public string DriverLicenseNumber { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Address { get; set; }

        public string Phone { get; set; }

        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
