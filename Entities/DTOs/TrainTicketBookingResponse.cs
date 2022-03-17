using Core.Entities;
using System.Collections.Generic;

namespace Entities.DTOs
{
    public class TrainTicketBookingResponse : IDto
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<ArrengementDetails> YerlesimAyrinti { get; set; }
    }
}
