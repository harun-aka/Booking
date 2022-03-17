using Core.Entities;

namespace Entities.DTOs
{
    public class VagonDto : IDto
    {
        public string Ad { get; set; }
        public int Kapasite { get; set; }
        public int DoluKoltukAdet { get; set; }

    }
}
