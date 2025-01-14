namespace ParkingApi.Interfaces
{
    public interface IAuthService
    {
        Task<string> GenerateToken();
    }
}
