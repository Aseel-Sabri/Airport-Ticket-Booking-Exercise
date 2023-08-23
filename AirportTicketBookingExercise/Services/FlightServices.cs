using AirportTicketBookingExercise.DTOs;
using AirportTicketBookingExercise.Models;
using AirportTicketBookingExercise.Repositories;
using AirportTicketBookingExercise.Validation;
using FluentResults;

namespace AirportTicketBookingExercise.Services;

public class FlightServices : IFlightServices
{
    private readonly IFlightRepository _flightRepository = new FlightRepository();

    public void BookFlight(int passengerId)
    {
        var flightId = GetId();
        var flightClass = GetClass();
        var bookingResult = _flightRepository.BookFlight(flightId, flightClass, passengerId);

        Console.WriteLine();
        if (bookingResult.IsFailed)
        {
            Console.WriteLine(bookingResult.Errors.First().Message);
            return;
        }

        Console.WriteLine("Booked Successfully");
    }

    public void SearchAvailableFlightsForBooking(int passengerId)
    {
        Console.WriteLine("Specify attributes to be used in search");
        Console.WriteLine("  Leave an attribute empty to ignore");
        var flightDetails = GetFlightDetails();
        var availableFlights = _flightRepository.GetAvailableFilteredFlights(flightDetails, passengerId).ToList();
        Console.WriteLine();
        if (!availableFlights.Any())
        {
            Console.WriteLine("No Available Flights");
            return;
        }

        foreach (var flight in availableFlights)
        {
            Console.WriteLine(flight);
        }
    }

    private FlightDto GetFlightDetails()
    {
        return new FlightDto
        {
            Price = GetPriceOrNull(),
            DepartureCountry = GetField(_ => Result.Ok(), "Departure Country"),
            DestinationCountry = GetField(_ => Result.Ok(), "Destination Country"),
            DepartureDate = GetDepartureDateOrNull(),
            DepartureAirport = GetField(_ => Result.Ok(), "Departure Airport"),
            ArrivalAirport = GetField(_ => Result.Ok(), "ArrivalAirport"),
            Class = GetClassOrNull()
        };
    }

    private double? GetPriceOrNull()
    {
        var priceString = GetField(ConsoleValidation.ValidateNullablePrice, "Price");
        return priceString == null ? null : Double.Parse(priceString);
    }

    private DateTime? GetDepartureDateOrNull()
    {
        var dateString = GetField(ConsoleValidation.ValidateNullableDate, "Departure Date");
        return dateString == null ? null : DateTime.Parse(dateString);
    }

    private FlightClass.ClassType? GetClassOrNull()
    {
        var classString = GetField(ConsoleValidation.ValidateNullableClassType, "Class");
        return classString == null ? null : Enum.Parse<FlightClass.ClassType>(classString);
    }

    private FlightClass.ClassType GetClass()
    {
        var classString = GetField(ConsoleValidation.ValidateClassType, "Class");
        return Enum.Parse<FlightClass.ClassType>(classString!);
    }

    private int GetId()
    {
        var idString = GetField(ConsoleValidation.ValidateId, "Flight ID");
        return int.Parse(idString!);
    }

    private static string? GetField(Func<string?, Result> fieldValidationMethod, string fieldName)
    {
        string? value;
        Result validationResult;
        do
        {
            Console.Write($"{fieldName}: ");
            value = Console.ReadLine();
            validationResult = fieldValidationMethod(value);
            if (validationResult.IsFailed)
            {
                Console.WriteLine(validationResult.Errors.First().Message);
            }
        } while (validationResult.IsFailed);

        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}