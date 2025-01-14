namespace ParkingApi.Models.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Parking
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int ParkingSpaces { get; set; }
    }
}
