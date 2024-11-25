using DbAccess.Context;
using Entities;
using CarStatus = Entities.CarStatus;

namespace MVCApp.Middleware
{
    public class DbSeederMiddleware
    {
        private readonly RequestDelegate _next;

        public DbSeederMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Context dbContext)
        {
            if (!dbContext.Cars.Any() || !dbContext.Owners.Any() || !dbContext.CarsStatuses.Any())
            {
                try
                {
                    var quantityEntity = 50;
                    var random = new Random();

                    var newCarsStatuses = new CarStatus[quantityEntity];

                    for (int i = 0; i < newCarsStatuses.Length; i++)
                        newCarsStatuses[i] = new CarStatus
                        {
                            StatusName = $"StatusName {i}"
                        };

                    await dbContext.CarsStatuses.AddRangeAsync(newCarsStatuses);

                    var newOwners = new Owner[quantityEntity];

                    for (int i = 0; i < newOwners.Length; i++)
                    {
                        var newOwner = new Owner
                        {
                            DriverLicenseNumber = $"DriverLicenseNumber {i}",
                            FullName = $"FullName {i}",
                            Address = $"Address {i}",
                            Phone = $"Phone {i}"
                        };

                        newOwners[i] = newOwner;
                    }

                    await dbContext.Owners.AddRangeAsync(newOwners);

                    dbContext.SaveChanges();

                    var newCars = new Car[quantityEntity];

                    for (int i = 0; i < newCars.Length; i++)
                    {
                        var newCar = new Car
                        {
                            LicensePlate = $"LicensePlate {i}",
                            Brand = $"Brand {i}",
                            Power = i,
                            Color = $"Color {i}",
                            YearOfProduction = i,
                            ChassisNumber = $"ChassisNumber {i}",
                            EngineNumber = $"EngineNumber {i}",
                            DateReceived = new DateTime(i * 10000),
                            OwnerId = newOwners[random.Next(1, 50)].Id,
                        };

                        newCars[i] = newCar;
                    }

                    await dbContext.Cars.AddRangeAsync(newCars);

                    dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            await _next(context);
        }
    }
}
