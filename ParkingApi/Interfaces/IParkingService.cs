namespace ParkingApi.Interfaces
{
    public interface IParkingService
    {
        Task<int?> GetVehicleCount(int id);

        Task<bool> DoesParkingExist(int id);

        Task<bool> IsCarParkedOnParking(int parkingId, string registration_number);

        Task<bool> IsValidLicensePlate(string registration_number);

        Task<bool> HasAvailableSpaces(int parkingId);

        Task<Guid> AddCarVehicleToParking(int parkingId, string registration_number);

        Task RemoveCarFromParking(int parkingId, string registration_number);
    }
}
