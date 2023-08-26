using AirportTicketBookingExercise.Models;
using FluentResults;

namespace AirportTicketBookingExercise.Validation;

public static class CsvValidation
{
    public static Result ValidateUserUniqueness(User userToValidate, List<User> users)
    {
        var errorsList = new List<string>();
        if (users.Any(user => user.Id == userToValidate.Id))
            errorsList.Add($"Duplicate User ID: User with ID {userToValidate.Id} already exists");

        if (users.Any(user => user.Username == userToValidate.Username))
            errorsList.Add($"Duplicate Username: User with username {userToValidate.Username} already exists");

        return errorsList.Any() ? Result.Fail(errorsList) : Result.Ok();
    }

    public static Result ValidateFlightUniqueness(Flight flightToValidate, List<Flight> flights)
    {
        return Result.FailIf(flights.Any(flight => flight.Id == flightToValidate.Id),
            () => $"Duplicate Flight ID: Flight with ID {flightToValidate.Id} already exists");
    }

    public static Result<FlightClass> ValidateFlightClassUniqueness(FlightClass flightClassToValidate,
        List<FlightClass> flightClassesList)
    {
        return Result.FailIf(flightClassesList.Any(flight => flight.Id == flightClassToValidate.Id),
            () => $"Duplicate FlightClass ID: FlightClass with ID {flightClassToValidate.Id} already exists");
    }

    public static Result<Booking> ValidateBookingUniqueness(Booking bookingToValidate, List<Booking> bookingsList)
    {
        var flight = bookingToValidate.FlightClass.Flight;
        var passenger = bookingToValidate.Passenger;
        var isDuplicate = bookingsList.Any(booking =>
            booking.Passenger == passenger
            && booking.FlightClass.Flight == flight);
        return Result.FailIf(isDuplicate,
            () =>
                $"Duplicate Booking: Flight with ID {flight.Id} already booked by passenger with ID {passenger.Id}");
    }

    public static bool IsPositiveInteger(string? input)
    {
        return int.TryParse(input, out var conversionOutput) && conversionOutput > 0;
    }

    public static bool IsPositiveDouble(string? input)
    {
        return double.TryParse(input, out var conversionOutput) && conversionOutput > 0;
    }
}