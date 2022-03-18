using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class TrainManager : ITrainService
    {
        public DataResult<TrainTicketBookingResponse> BookTrainTickets(TrainTicketBookingRequest trainTicketBookingRequest)
        {

            TrainTicketBookingResponse trainTicketBookingResponse = new TrainTicketBookingResponse();
            trainTicketBookingResponse.YerlesimAyrinti = new List<ArrengementDetails>();

            if (trainTicketBookingRequest == null)
            {
                return new ErrorDataResult<TrainTicketBookingResponse>(trainTicketBookingResponse, Messages.RequestIsNull);
            }

            if(trainTicketBookingRequest.Tren.Vagonlar == null)
            {
                return new ErrorDataResult<TrainTicketBookingResponse>(trainTicketBookingResponse, Messages.RequestIsNull);
            }

            List<string> availableSeatVagonNames = new List<string>();
            List<int> availableSeatCountsPerVagon = new List<int>();
            int unBookedSeatCount = GetUnBookedSeatCountTotal(availableSeatVagonNames, availableSeatCountsPerVagon, trainTicketBookingRequest.Tren.Vagonlar);

            if (unBookedSeatCount < trainTicketBookingRequest.RezervasyonYapilacakKisiSayisi)
            {
                trainTicketBookingResponse.RezervasyonYapilabilir = false;
                return new SuccessDataResult<TrainTicketBookingResponse>(trainTicketBookingResponse);
            }

            trainTicketBookingResponse.RezervasyonYapilabilir = true;
            if (trainTicketBookingRequest.KisilerFarkliVagonlaraYerlestirilebilir is true)
            {
                BookForAnyVagon(trainTicketBookingRequest, trainTicketBookingResponse, availableSeatVagonNames, availableSeatCountsPerVagon);
                return new SuccessDataResult<TrainTicketBookingResponse>(trainTicketBookingResponse);
            }

            BookForOnlyOneVagon(trainTicketBookingRequest, trainTicketBookingResponse, availableSeatVagonNames, availableSeatCountsPerVagon);

            return new SuccessDataResult<TrainTicketBookingResponse>(trainTicketBookingResponse);
        }

        private static void BookForOnlyOneVagon(TrainTicketBookingRequest trainTicketBookingRequest, TrainTicketBookingResponse trainTicketBookingResponse, List<string> availableSeatVagonNames, List<int> availableSeatCountsPerVagon)
        {
            string availableVagonName = null;
            for (int i = 0; i < availableSeatVagonNames.Count; i++)
            {
                if (availableSeatCountsPerVagon[i] > trainTicketBookingRequest.RezervasyonYapilacakKisiSayisi)
                {
                    availableVagonName = availableSeatVagonNames[i];
                    break;
                }
            }

            if (!string.IsNullOrEmpty(availableVagonName))
            {

                trainTicketBookingResponse.YerlesimAyrinti.Add(new ArrengementDetails
                {
                    VagonAdi = availableVagonName,
                    KisiSayisi = trainTicketBookingRequest.RezervasyonYapilacakKisiSayisi
                });
            }
        }

        private static void BookForAnyVagon(TrainTicketBookingRequest trainTicketBookingRequest, TrainTicketBookingResponse trainTicketBookingResponse, List<string> availableSeatVagonNames, List<int> availableSeatCountsPerVagon)
        {
            var arrayIndex = 0;
            while (trainTicketBookingRequest.RezervasyonYapilacakKisiSayisi > 0)
            {
                int availableSeatCount = availableSeatCountsPerVagon[arrayIndex];
                var arrengementDetails = new ArrengementDetails();
                arrengementDetails.VagonAdi = availableSeatVagonNames[arrayIndex];
                arrengementDetails.KisiSayisi = trainTicketBookingRequest.RezervasyonYapilacakKisiSayisi > availableSeatCount ? availableSeatCount : trainTicketBookingRequest.RezervasyonYapilacakKisiSayisi;
                trainTicketBookingResponse.YerlesimAyrinti.Add(arrengementDetails);
                trainTicketBookingRequest.RezervasyonYapilacakKisiSayisi -= arrengementDetails.KisiSayisi;
                arrayIndex++;
            }
        }

        private int GetUnBookedSeatCountTotal(List<string> availableSeatVagonNames, List<int> availableSeatCountsPerVagon, List<VagonDto> vagonlar)
        {
            int availableSeatCountTotal = 0;
            vagonlar.ForEach(vagon =>
           {
               int availableVagonSeatCount = GetUnBookedSeatCountForAVagon(vagon);
               if (availableVagonSeatCount > 0)
               {
                   availableSeatCountTotal += availableVagonSeatCount;
                   availableSeatVagonNames.Add(vagon.Ad);
                   availableSeatCountsPerVagon.Add(availableVagonSeatCount);
               }
           });
            return availableSeatCountTotal;
        }

        private int GetUnBookedSeatCountForAVagon(VagonDto vagon)
        {
            int unBookedSeatCount = (int)(vagon.Kapasite * 0.7) - vagon.DoluKoltukAdet;
            if (unBookedSeatCount > 0)
            {
                return unBookedSeatCount;
            }
            return 0;
        }
    }
}
