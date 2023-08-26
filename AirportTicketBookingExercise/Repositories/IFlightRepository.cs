using AirportTicketBookingExercise.DTOs;
using AirportTicketBookingExercise.Models;
using FluentResults;

namespace AirportTicketBookingExercise.Repositories;

public interface IFlightRepository
{
    IEnumerable<FlightClass> GetAvailableFilteredFlights(FlightDto flightDto, int passengerId);
    Result BookFlight(int flightId, FlightClass.ClassType flightClass, int passengerId);
    Result EditBooking(int flightId, FlightClass.ClassType flightClass, int passengerId);
    Result CancelBooking(int flightId, int passengerId);
    IEnumerable<Booking> GetPassengerBookings(int passengerId);
    IEnumerable<Booking> GetAllBookings();
    Result LoadFlights(string filePath);
    Result LoadFlightsClasses(string flightClassFilePath);
}