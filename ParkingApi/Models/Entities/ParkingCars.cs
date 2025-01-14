namespace ParkingApi.Models.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class ParkingCars
    {
        [Key]
        public Guid Id { get; set; }

        public int ParkingId { get; set; }

        public Guid CarId { get; set; }

        public DateTime ParkDate { get; set; }

        public Parking Parking { get; set; }

        public Cars Cars { get; set; }
    }
}
