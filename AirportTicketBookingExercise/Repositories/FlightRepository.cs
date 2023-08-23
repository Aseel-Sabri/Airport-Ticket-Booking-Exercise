using AirportTicketBookingExercise.DTOs;
using AirportTicketBookingExercise.Models;

namespace AirportTicketBookingExercise.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly List<User> _users;
    private readonly List<Flight> _flights;
    private readonly List<FlightClass> _flightClasses;
    private readonly List<Booking> _bookings;

    public FlightRepository()
    {
        IDataLoader dataLoader = CsvDataLoader.Instance;
        _users = dataLoader.Users;
        _flights = dataLoader.Flights;
        _flightClasses = dataLoader.FlightClasses;
        _bookings = dataLoader.Bookings;
    }

    public IEnumerable<FlightClass> GetAvailableFilteredFlights(FlightDto flightDto, int passengerId)
    {
        var flightsBookedByPassenger = _bookings.Where(booking => booking.Passenger.Id == passengerId)
            .Select(booking => booking.FlightClass.Flight);

        var availableFlights = _flightClasses
            .Where(flightClass => flightClass.GetAvailableNumberOfSeats() > 0)
            .Where(flightClass => flightsBookedByPassenger.All(flight => flight.Id != flightClass.Flight.Id));

        if (flightDto.Price != null)
            availableFlights = availableFlights.Where(flightClass => flightClass.Price == flightDto.Price);

        if (flightDto.DepartureCountry != null)
            availableFlights = availableFlights.Where(flightClass =>
                flightClass.Flight.DepartureCountry.Contains(flightDto.DepartureCountry));

        if (flightDto.DestinationCountry != null)
            availableFlights = availableFlights.Where(flightClass =>
                flightClass.Flight.DestinationCountry.Contains(flightDto.DestinationCountry));

        if (flightDto.DepartureDate != null)
            availableFlights =
                availableFlights.Where(flightClass => flightClass.Flight.DepartureDate == flightDto.DepartureDate);

        if (flightDto.DepartureAirport != null)
            availableFlights = availableFlights.Where(flightClass =>
                flightClass.Flight.DepartureAirport.Contains(flightDto.DepartureAirport));

        if (flightDto.ArrivalAirport != null)
            availableFlights = availableFlights.Where(flightClass =>
                flightClass.Flight.ArrivalAirport.Contains(flightDto.ArrivalAirport));

        if (flightDto.Class != null)
            availableFlights = availableFlights.Where(flightClass => flightClass.Type == flightDto.Class);

        return availableFlights;
    }
}