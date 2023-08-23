using AirportTicketBookingExercise.DTOs;
using AirportTicketBookingExercise.Models;
using FluentResults;

namespace AirportTicketBookingExercise.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly IUserRepository _userRepository = new UserRepository();

    private readonly List<Flight> _flights;
    private readonly List<FlightClass> _flightClasses;
    private readonly List<Booking> _bookings;

    public FlightRepository()
    {
        IDataLoader dataLoader = CsvDataLoader.Instance;
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

        var flightBooked = IsFlightBooked(flightId, passengerId);
        if (flightBooked.IsSuccess)
            return Result.Fail(
                $"Booking Failed: A seat in flight {flightId} already Booked");

        if (!HasAvailableSeats(flightId, flightClass))
            return Result.Fail($"Booking Failed: No available {flightClass} seats to book in the flight");

        AddBooking();
        return Result.Ok();

        #region local functions

        void AddBooking()
        {
            var flightToBook = _flightClasses
                .Where(flight => flight.Flight.Id == flightId)
                .Where(flight => flight.Type == flightClass)
                .ToList().First();

            var passengerResult = _userRepository.GetUserById(passengerId);
            flightToBook.PassengerCount++;
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

    private Result IsFlightBooked(int flightId, int passengerId)
    {
        var bookedFlight = _bookings.Where(booking =>
            booking.FlightClass.Flight.Id == flightId && booking.Passenger.Id == passengerId).ToList();

        if (bookedFlight.Any())
        {
            return Result.Ok();
        }

        return Result.Fail(
            $"No seat was booked in flight {flightId}");
    }

    private bool HasAvailableSeats(int flightId, FlightClass.ClassType flightClass)
    {
        var flight = _flightClasses
            .Where(flight => flight.Flight.Id == flightId)
            .Where(flight => flight.Type == flightClass)
            .ToList();

        return flight.Any() && flight.First().GetAvailableNumberOfSeats() > 0;
    }

    private bool HasFlight(int flightId)
    {
        return _flights.Any(flight => flight.Id == flightId);
    }

    public Result EditBooking(int flightId, FlightClass.ClassType flightClass, int passengerId)
    {
        if (!HasFlight(flightId))
            return Result.Fail($"Edit Failed: No flight with ID {flightId} Exists");

        var isFlightBooked = IsFlightBooked(flightId, passengerId);
        if (isFlightBooked.IsFailed)
            return Result.Fail($"Edit Failed: {isFlightBooked.Errors.First().Message}");

        if (!HasAvailableSeats(flightId, flightClass))
            return Result.Fail($"Edit Failed: No available {flightClass} seats in flight {flightId}");

        UpdateBooking();
        return Result.Ok();

        #region localfunction

        void UpdateBooking()
        {
            var bookedFlight = _bookings.Where(booking =>
                booking.FlightClass.Flight.Id == flightId && booking.Passenger.Id == passengerId).ToList().First();

            var flightToBook = _flightClasses
                .First(flight => flight.Flight.Id == flightId
                                 && flight.Type == flightClass);

            bookedFlight.FlightClass.PassengerCount--;
            bookedFlight.FlightClass = flightToBook;
            flightToBook.PassengerCount++;
        }

        #endregion
    }

    public Result CancelBooking(int flightId, int passengerId)
    {
        if (!HasFlight(flightId))
            return Result.Fail($"Cancel Failed: No flight with ID {flightId} Exists");

        var isFlightBooked = IsFlightBooked(flightId, passengerId);
        if (isFlightBooked.IsFailed)
            return Result.Fail($"Cancel Failed: {isFlightBooked.Errors.First().Message}");

        var bookedFlight = _bookings.Where(booking =>
            booking.FlightClass.Flight.Id == flightId && booking.Passenger.Id == passengerId).ToList().First();

        bookedFlight.FlightClass.PassengerCount--;
        _bookings.Remove(bookedFlight);
        return Result.Ok();
    }

    public IEnumerable<Booking> GetPassengerBookings(int passengerId)
    {
        return _bookings
            .Where(booking => booking.Passenger.Id == passengerId);
    }

    public IEnumerable<Booking> GetAllBookings()
    {
        return _bookings.AsEnumerable();
    }
}