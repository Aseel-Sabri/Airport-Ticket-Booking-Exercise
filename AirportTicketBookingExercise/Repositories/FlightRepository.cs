using AirportTicketBookingExercise.DTOs;
using AirportTicketBookingExercise.Models;
using FluentResults;

namespace AirportTicketBookingExercise.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly IUserRepository _userRepository = new UserRepository();

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

    public Result BookFlight(int flightId, FlightClass.ClassType flightClass, int passengerId)
    {
        if (!HasFlight(flightId))
            return Result.Fail($"Booking Failed: No flight with ID {flightId} Exists");

        var flightNotBooked = CheckIfFlightNotBooked();
        if (flightNotBooked.IsFailed)
            return flightNotBooked;

        var flightToBookList = _flightClasses
            .Where(flight => flight.Flight.Id == flightId)
            .Where(flight => flight.Type == flightClass)
            .ToList();

        var hasAvailableSeats = HasAvailableSeats();
        if (hasAvailableSeats.IsFailed)
            return hasAvailableSeats;

        AddBooking(flightToBookList.First());
        return Result.Ok();

        #region local functions

        Result CheckIfFlightNotBooked()
        {
            var bookedFlight = _bookings.Where(booking =>
                booking.FlightClass.Flight.Id == flightId && booking.Passenger.Id == passengerId).ToList();

            if (bookedFlight.Any())
            {
                if (bookedFlight.First().FlightClass.Type == flightClass)
                    return Result.Fail($"Booking Failed: Flight already booked");

                return Result.Fail(
                    $"Booking Failed: A seat in the {bookedFlight.First().FlightClass.Type} of the flight already Booked");
            }

            return Result.Ok();
        }

        Result HasAvailableSeats()
        {
            if (!flightToBookList!.Any() || flightToBookList!.First().GetAvailableNumberOfSeats() == 0)
            {
                return Result.Fail($"Booking Failed: No available {flightClass} seats to book in the flight");
            }

            return Result.Ok();
        }

        void AddBooking(FlightClass flightToBook)
        {
            var passengerResult = _userRepository.GetUserById(passengerId);
            flightToBook.Passengers.Add(passengerResult.Value);
            _bookings.Add(
                new Booking()
                {
                    Passenger = passengerResult.Value,
                    FlightClass = flightToBook
                }
            );
        }

        #endregion
    }

    private bool HasFlight(int flightId)
    {
        return _flights.Any(flight => flight.Id == flightId);
    }
}