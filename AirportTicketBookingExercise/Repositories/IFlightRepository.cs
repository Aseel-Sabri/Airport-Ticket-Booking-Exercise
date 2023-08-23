using AirportTicketBookingExercise.DTOs;
using AirportTicketBookingExercise.Models;

namespace AirportTicketBookingExercise.Repositories;

public interface IFlightRepository
{
    IEnumerable<FlightClass> GetAvailableFilteredFlights(FlightDto flightDto, int passengerId);
}