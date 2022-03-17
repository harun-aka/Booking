using Core.Entities;
using System;
using System.Text;

namespace Entities.DTOs
{
    public class TrainTicketBookingRequest : IDto
    {
        public TrainDto Tren { get; set; }
        public int RezervasyonYapilacakKisiSayisi { get; set; }
        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }
    }
}
