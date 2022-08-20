using AutoMapper;
using HotelBookingAPI.Data;
using HotelBookingAPI.Models;
using HotelBookingAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using HotelBookingAPI;
using HotelBookingAPI.Helpers;

namespace HotelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingController(ILogger<BookingController> logger, IBookingRepository bookingRepository, IMapper mapper)
        {
            _logger = logger;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        [HttpPost(Name = nameof(CreateBooking))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateBooking)}");
                return BadRequest(ModelState);
            }
            if (Utils.CheckOutIsGreaterThanThirtyDays(bookingDTO.CheckOutDate) || 
                Utils.DateBetweenIsGreaterThanThreeDays(bookingDTO.CheckInDate, bookingDTO.CheckOutDate) ||
                Utils.CheckInIsGreaterThanOrEqualCheckOut(bookingDTO.CheckInDate, bookingDTO.CheckOutDate))
            {
                _logger.LogInformation("Invalid date range");
                return BadRequest("Invalid date range");
            }

            if (await _bookingRepository.IsBooked(bookingDTO.CheckInDate, bookingDTO.CheckOutDate))
            {
                _logger.LogInformation("Attempt to reserve dates that are not available");
                return BadRequest("The chosen dates are not available");
            }

            try
            {
                var booking = _bookingRepository.CreateBooking(bookingDTO);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(CreateBooking)}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id:int}", Name = nameof(UpdateBooking))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingDTO bookingDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Update attempt in {nameof(UpdateBooking)}");
                return BadRequest(ModelState);
            }

            if (Utils.CheckOutIsGreaterThanThirtyDays(bookingDTO.CheckOutDate) ||
               Utils.DateBetweenIsGreaterThanThreeDays(bookingDTO.CheckInDate, bookingDTO.CheckOutDate) ||
               Utils.CheckInIsGreaterThanOrEqualCheckOut(bookingDTO.CheckInDate, bookingDTO.CheckOutDate))
            {
                _logger.LogInformation("Invalid date range");
                return BadRequest("Invalid date range");
            }

            if (await _bookingRepository.IsBooked(bookingDTO.CheckInDate, bookingDTO.CheckOutDate, id))
            {
                _logger.LogInformation("Attempted to reserve dates that aren't available");
                return BadRequest("The chosen dates aren't available");
            }

            try
            {
                var booking = await _bookingRepository.Get(x => x.Id == id);
                if(booking== null)
                {
                    _logger.LogError($"Submitted data invalid in {nameof(UpdateBooking)}");
                    return BadRequest("Submited data is invalid");
                }
                _mapper.Map(bookingDTO, booking);
                var updatedBooking = await _bookingRepository.UpdateBooking(id, booking);
                return Ok(updatedBooking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(UpdateBooking)}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id:int}", Name = nameof(DeleteBooking))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid Delete attempt in {nameof(DeleteBooking)}");
                return BadRequest();
            }
            try
            {
                await _bookingRepository.DeleteBooking(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {nameof(DeleteBooking)}");
                return StatusCode(500, "Internal server error. Please try again later");
            }
        }
    }
}
