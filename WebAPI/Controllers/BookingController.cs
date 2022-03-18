using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        ITrainService _trainService;

        public BookingController(ITrainService trainService)
        {
            _trainService = trainService;
        }
        [HttpPost("booktraintickets")]
        public IActionResult BookTrainTickets(TrainTicketBookingRequest trainTicketBookingRequest)
        {
            var result = _trainService.BookTrainTickets(trainTicketBookingRequest);
            if(result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
            
        }
    }
}
