
namespace WaveApp.Models
{
    public class Buoy
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Status { get; set; } // Activa, Desactiva, En Mantenimiento
    }
}
