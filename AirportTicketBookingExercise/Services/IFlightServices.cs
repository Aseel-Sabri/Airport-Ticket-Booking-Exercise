using AirportTicketBookingExercise.Models;

namespace AirportTicketBookingExercise.Services;

public interface IFlightServices
{
    void SearchAvailableFlightsForBooking(int passengerId);
    void BookFlight(int passengerId);
}