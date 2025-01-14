namespace ParkingApi.Services
{
    using ParkingApi.Interfaces;
    using ParkingApi.Models.DbContexts;
    using ParkingApi.Models.Entities;
    using System.Data.Entity;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class ParkingService : IParkingService
    {
        private readonly ParkingDbContext _parkingDbContext;
        public ParkingService(ParkingDbContext parkingDbContext)
        {
            _parkingDbContext = parkingDbContext;
        }

        public async Task<bool> DoesParkingExist(int id)
        {
            var parking = _parkingDbContext.Parking.Find(id);

            if (parking == null)
            {
                return false;
            }

            return true;
        }

        public async Task<int?> GetVehicleCount(int id)
        {
            var parkingCars = _parkingDbContext.ParkingCars
                .AsNoTracking()
                .Where(x => x.ParkingId == id)
                .ToList();

            var parkingCarsCount = parkingCars.Count;

            return parkingCarsCount;
        }

        public async Task<Guid> AddCarVehicleToParking(int parkingId, string registration_number)
        {
            var newVehicle = await this.AddVehicle(registration_number);

            var addCarToParking = new ParkingCars
            {
                CarId = newVehicle,
                ParkingId = parkingId,
                ParkDate = DateTime.Now,
            };

            _parkingDbContext.ParkingCars.Add(addCarToParking);
            _parkingDbContext.SaveChanges();

            return addCarToParking.Id;
        }

        private async Task<Guid> AddVehicle(string registration_number)
        {
            var newVehicle = new Cars
            {
                Id = Guid.NewGuid(),
                Plate = registration_number,
            };

            _parkingDbContext.Add(newVehicle);
            _parkingDbContext.SaveChanges();

            return newVehicle.Id;
        }

        public async Task<bool> IsCarParkedOnParking(int parkingId, string registration_number)
        {
            var isCarParkedOnParking = _parkingDbContext.ParkingCars
                .Include(pc => pc.Cars)
                .AsNoTracking()
                .FirstOrDefault(pc => pc.Cars.Plate == registration_number && pc.ParkingId == parkingId);

            if (isCarParkedOnParking == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsValidLicensePlate(string registration_number)
        {
            if (!Regex.IsMatch(registration_number, "^[A-Z0-9]{1,8}$"))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> HasAvailableSpaces(int parkingId)
        {
            var howManySpaces = _parkingDbContext.Parking
                .AsNoTracking()
                .Where(x => x.Id == parkingId)
                .Select(x => x.ParkingSpaces)
                .FirstOrDefault();

            var carsOnParking = _parkingDbContext.ParkingCars
                .AsNoTracking()
                .Where(x => x.ParkingId == parkingId)
                .Select(x => x.CarId)
                .ToList();

            var countCarsOnParking = carsOnParking.Count();

            if (countCarsOnParking + 1 <= howManySpaces)
            {
                return true;
            }

            return false;
        }

        public async Task RemoveCarFromParking(int parkingId, string registration_number)
        {
            var carInParking = _parkingDbContext.ParkingCars.Where(x => x.ParkingId == parkingId && x.Cars.Plate.Equals(registration_number)).FirstOrDefault();

            _parkingDbContext.ParkingCars.Remove(carInParking);
            _parkingDbContext.SaveChanges();
        }
    }
}
