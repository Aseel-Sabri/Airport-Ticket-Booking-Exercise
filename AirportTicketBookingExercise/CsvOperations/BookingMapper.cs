using AirportTicketBookingExercise.Models;
using AirportTicketBookingExercise.Validation;
using CsvHelper;
using CsvHelper.Configuration;
using FluentResults;

namespace AirportTicketBookingExercise.CsvOperations;

public class BookingMapper : IEntityMapper<Booking>
{
    public Result<Booking> GetEntity(IReaderRow csvReader, List<Booking> bookingsList)
    {
        var passengerResult = GetPassenger(csvReader);
        var flightClassResult = GetFlightClass(csvReader);
        var bookingResult = Result.Merge(passengerResult, flightClassResult);

        if (bookingResult.IsFailed)
            return bookingResult;

        var booking = new Booking()
        {
            Passenger = passengerResult.Value,
            FlightClass = flightClassResult.Value
        };

        var bookingUniquenessResult = CsvValidation.ValidateBookingUniqueness(booking, bookingsList);
        return bookingUniquenessResult.IsFailed ? bookingUniquenessResult : Result.Ok(booking);
    }

    private Result<User> GetPassenger(IReaderRow csvReader)
    {
        #region ErrorDecleration

        var errorDetails = $"""
                              * Required
                              * Positive Integer
                              * Correspond to An Existing Passenger
                            """;
        var errorMessage = ErrorMessage.GenerateFieldErrorMessage("PassengerId", "Booking", errorDetails);

        #endregion

        var fieldValue = csvReader.GetField("PassengerId");
        if (!BasicValidation.IsPositiveInteger(fieldValue))
        {
            return Result.Fail(errorMessage);
        }

        var passengerId = int.Parse(fieldValue!);

        var users = CsvDataManager.Instance.Users;
        var passenger = users.FirstOrDefault(user =>
            user.Id == passengerId);

        if (passenger is null)
        {
            return Result.Fail(errorMessage);
        }

        return Result.Ok(passenger);
    }

    private Result<FlightClass> GetFlightClass(IReaderRow csvReader)
    {
        #region ErrorDecleration

        var errorDetails = $"""
                              * Required
                              * Positive Integer
                              * Correspond to An Existing Flight
                            """;
        var errorMessage = ErrorMessage.GenerateFieldErrorMessage("FlightClassId", "Booking", errorDetails);

        #endregion

        var flightClasses = CsvDataManager.Instance.FlightClasses;
        var fieldValue = csvReader.GetField("FlightClassId");
        if (!BasicValidation.IsPositiveInteger(fieldValue))
        {
            return Result.Fail(errorMessage);
        }

        var flightId = int.Parse(fieldValue!);
        var flightClass = flightClasses.FirstOrDefault(flight => flight.Id == flightId);
        if (flightClass is null)
        {
            return Result.Fail(errorMessage);
        }

        return Result.Ok(flightClass);
    }

    public class BookingMap : ClassMap<Booking>
    {
        public BookingMap()
        {
            Map(m => m.FlightClass)
                .Convert(convertToStringFunction: args => args.Value.FlightClass.Id.ToString())
                .Name("FlightClassId");

            Map(m => m.Passenger)
                .Convert(convertToStringFunction: args => args.Value.Passenger.Id.ToString())
                .Name("PassengerId");
        }
    }
}