using AirportTicketBookingExercise.Models;

namespace AirportTicketBookingExercise.Services;

public interface IFlightServices
{
    void SearchAvailableFlightsForBooking(int passengerId);
    void BookFlight(int passengerId);
    void EditBooking(int passengerId);
    void CancelBooking(int passengerId);
    void ViewPassengerBookings(int passengerId);
}