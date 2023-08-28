using AirportTicketBookingExercise.Models;
using AirportTicketBookingExercise.Validation;
using CsvHelper;
using CsvHelper.Configuration;
using FluentResults;
using static AirportTicketBookingExercise.Models.FlightClass.ClassType;

namespace AirportTicketBookingExercise.CsvOperations;

public class FlightClassMapper : IEntityMapper<FlightClass>
{
    public Result<FlightClass> GetEntity(IReaderRow csvReader, List<FlightClass> flightClassesList)
    {
        var idResult = GetId(csvReader);
        var capacityResult = GetCapacity(csvReader);
        var priceResult = GetPrice(csvReader);
        var classTypeResult = GetClassType(csvReader);
        var flightResult = GetFlight(csvReader);
        var flightClassResult = Result.Merge(idResult, capacityResult, priceResult, flightResult);
        if (flightClassResult.IsFailed)
            return flightClassResult;
        var flightClass = new FlightClass()
        {
            Id = idResult.Value,
            Capacity = capacityResult.Value,
            Price = priceResult.Value,
            Type = classTypeResult.Value,
            Flight = flightResult.Value
        };
        var flightClassUniquenessResult = CsvValidation.ValidateFlightClassUniqueness(flightClass, flightClassesList);
        return flightClassUniquenessResult.IsFailed ? flightClassUniquenessResult : Result.Ok(flightClass);
    }

    private Result<int> GetId(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("Id");
        if (BasicValidation.IsPositiveInteger(fieldValue))
        {
            var id = int.Parse(fieldValue!);
            return Result.Ok(id);
        }

        var details = """
                        * Required
                        * Positive Integer
                        * Unique
                      """;
        var errorMessage = ErrorMessage.GenerateFieldErrorMessage("Id", "FlightClass", details);

        return Result.Fail(errorMessage);
    }

    private Result<int> GetCapacity(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("Capacity");
        if (BasicValidation.IsPositiveInteger(fieldValue))
        {
            var capacity = int.Parse(fieldValue!);
            return Result.Ok(capacity);
        }

        var details = """
                        * Required
                        * Positive Integer
                      """;
        var errorMessage = ErrorMessage.GenerateFieldErrorMessage("Capacity", "FlightClass", details);

        return Result.Fail(errorMessage);
    }

    private Result<double> GetPrice(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("Price");
        if (BasicValidation.IsPositiveDouble(fieldValue))
        {
            var price = double.Parse(fieldValue!);
            return Result.Ok(price);
        }

        var details = """
                        * Required
                        * Positive Double
                      """;
        var errorMessage = ErrorMessage.GenerateFieldErrorMessage("Price", "FlightClass", details);

        return Result.Fail(errorMessage);
    }

    private Result<FlightClass.ClassType> GetClassType(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("Type");
        if (Enum.TryParse(fieldValue, out FlightClass.ClassType type)) return Result.Ok(type);

        var details = $"""
                         * Required
                         * Value in ({EconomyClass}, {BusinessClass}, {FirstClass})
                       """;
        var errorMessage = ErrorMessage.GenerateFieldErrorMessage("Type", "FlightClass", details);

        return Result.Fail(errorMessage);
    }

    private Result<Flight> GetFlight(IReaderRow csvReader)
    {
        #region ErrorDecleration

        var errorDetails = $"""
                              * Required
                              * Positive Integer
                              * Correspond to An Existing Flight
                            """;
        var errorMessage = ErrorMessage.GenerateFieldErrorMessage("FlightId", "FlightClass", errorDetails);

        #endregion

        var fieldValue = csvReader.GetField<string>("FlightId");
        if (!BasicValidation.IsPositiveInteger(fieldValue))
            return Result.Fail(errorMessage);

        var id = int.Parse(fieldValue!);
        var flights = CsvDataManager.Instance.Flights;
        var flight = flights.FirstOrDefault(flight =>
            flight.Id == id);

        if (flight is null)
            return Result.Fail(errorMessage);

        return Result.Ok(flight);
    }


    public class FlightClassMap : ClassMap<FlightClass>
    {
        public FlightClassMap()
        {
            Map(m => m.Id);
            Map(m => m.Flight)
                .Convert(convertToStringFunction: args => args.Value.Flight.Id.ToString())
                .Name("FlightId");
            Map(m => m.Type);
            Map(m => m.Price);
            Map(m => m.Capacity);
        }
    }
}