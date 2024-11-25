using Entities.Base;

namespace Entities
{
    public class Car : EntityBase
    {
        public string LicensePlate { get; set; }

        public string Brand { get; set; }

        public int Power { get; set; }

        public string Color { get; set; }

        public int YearOfProduction { get; set; }

        public string ChassisNumber { get; set; }

        public string EngineNumber { get; set; }

        public DateTime DateReceived { get; set; }


        public Guid? OwnerId { get; set; }

        public Owner? Owner { get; set; }
    }
}
