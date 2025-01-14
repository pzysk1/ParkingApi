namespace ParkingApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ParkingApi.Interfaces;

    [ApiController]
    [Route("api/parking")]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingService _parkingService;
        public ParkingController(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [AllowAnonymous]
        [HttpGet("parked_cars_count/{id}")]
        public async Task<IActionResult> GetParkedCarsCount(int id)
        {
            var parkingExist = await _parkingService.DoesParkingExist(id);

            if (!parkingExist)
            {
                return NotFound("Parking not exist.");
            }

            var vehicleCount = await _parkingService.GetVehicleCount(id);

            return Ok(vehicleCount);
        }

        [Authorize]
        [HttpPost("add_vehicle/{parkingId}/{registration_number}")]
        public async Task<IActionResult> AddVehicleToParking(int parkingId, string registration_number)
        {
            var registrationNumberIsCorrect = await _parkingService.IsValidLicensePlate(registration_number);

            if (!registrationNumberIsCorrect)
            {
                return BadRequest("Invalid registration number format.");
            }

            var carOnParking = await _parkingService.IsCarParkedOnParking(parkingId, registration_number);

            if (carOnParking)
            {
                return BadRequest("Car is parked here.");
            }

            var checkAvaliablePlaces = await _parkingService.HasAvailableSpaces(parkingId);

            if (!checkAvaliablePlaces)
            {
                return BadRequest("No available spaces on this parking.");
            }

            var addCarToParking = await _parkingService.AddCarVehicleToParking(parkingId, registration_number);

            if (addCarToParking == null)
            {
                return BadRequest("Error while adding the car to the parking lot.");
            }

            return Ok("Car added to parking.");
        }

        [Authorize]
        [HttpDelete("remove_vehicle/{parkingId}/{registration_number}")]
        public async Task<IActionResult> RemoveVehicleFromParking(int parkingId, string registration_number)
        {
            var carOnParking = await _parkingService.IsCarParkedOnParking(parkingId, registration_number);

            if (!carOnParking)
            {
                return BadRequest("Car is not parked here.");
            }

            await _parkingService.RemoveCarFromParking(parkingId, registration_number);

            return Ok("Car removed from parking.");
        }
    }
}
