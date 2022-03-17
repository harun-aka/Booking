using Core.Entities;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class TrainDto : IDto
    {
        public string Ad { get; set; }
        public List<VagonDto> Vagonlar { get; set; } 
    }
}
