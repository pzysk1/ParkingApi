namespace ParkingApi.Models.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Cars
    {
        [Key]
        public Guid Id { get; set; }

        public string Plate { get; set; }
    }
}
