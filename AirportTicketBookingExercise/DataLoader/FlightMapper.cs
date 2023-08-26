using AirportTicketBookingExercise.Models;
using AirportTicketBookingExercise.Validation;
using CsvHelper;
using FluentResults;

namespace AirportTicketBookingExercise.DataLoader;

public class FlightMapper : IEntityMapper<Flight>
{
    public Result<Flight> GetEntity(IReaderRow csvReader, List<Flight> flightsList)
    {
        var idResult = GetId(csvReader);
        var departureCountryResult = GetDepartureCountry(csvReader);
        var destinationCountryResult = GetDestinationCountry(csvReader);
        var departureDateResult = GetDepartureDate(csvReader);
        var departureAirportResult = GetDepartureAirport(csvReader);
        var arrivalAirportResult = GetArrivalAirport(csvReader);

        var flightResult = Result.Merge(idResult, departureCountryResult, destinationCountryResult, departureDateResult,
            departureAirportResult, arrivalAirportResult);

        if (flightResult.IsFailed)
            return flightResult;

        var flight = new Flight()
        {
            Id = idResult.Value,
            DepartureCountry = departureCountryResult.Value,
            DestinationCountry = destinationCountryResult.Value,
            DepartureDate = departureDateResult.Value,
            DepartureAirport = departureAirportResult.Value,
            ArrivalAirport = arrivalAirportResult.Value
        };
        var flightUniquenessResult = CsvValidation.ValidateFlightUniqueness(flight, flightsList);
        return flightUniquenessResult.IsFailed ? flightUniquenessResult : Result.Ok(flight);
    }

    private Result<int> GetId(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("Id");
        if (BasicValidation.IsPositiveInteger(fieldValue))
        {
            var id = int.Parse(fieldValue!);
            return Result.Ok(id);
        }

        var errorMessage = """
                           Bad Data Encountered in 'Id' Field

                           Flight ID:
                            * Required
                            * Positive Integer
                            * Unique
                                                    
                           """;
        return Result.Fail(errorMessage);
    }

    private Result<string> GetDepartureCountry(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("DepartureCountry");
        if (!string.IsNullOrWhiteSpace(fieldValue)) return Result.Ok(fieldValue);

        var errorMessage = """
                           Bad Data Encountered in 'DepartureCountry' Field

                           Flight Departure Country:
                            * Required
                            * Non-Empty Freetext
                                                    
                           """;
        return Result.Fail(errorMessage);
    }

    private Result<string> GetDestinationCountry(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("DepartureCountry");
        if (!string.IsNullOrWhiteSpace(fieldValue)) return Result.Ok(fieldValue);

        var errorMessage = """
                           Bad Data Encountered in 'DestinationCountry' Field

                           Flight Destination Country:
                            * Required
                            * Non-Empty Freetext
                                                    
                           """;
        return Result.Fail(errorMessage);
    }
    //DepartureDate

    private Result<DateTime> GetDepartureDate(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("DepartureDate");
        if (DateTime.TryParse(fieldValue, out DateTime departureDate))
        {
            return Result.Ok(departureDate);
        }

        var errorMessage = """
                           Bad Data Encountered in 'DepartureDate' Field

                           Flight Departure Date:
                            * Required
                            * Date Format DD/MM/YYYY
                                                    
                           """;
        return Result.Fail(errorMessage);
    }

    private Result<string> GetDepartureAirport(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("DepartureAirport");
        if (!string.IsNullOrWhiteSpace(fieldValue)) return Result.Ok(fieldValue);

        var errorMessage = """
                           Bad Data Encountered in 'DepartureAirport' Field

                           Flight Departure Airport:
                            * Required
                            * Non-Empty Freetext
                                                    
                           """;
        return Result.Fail(errorMessage);
    }

    private Result<string> GetArrivalAirport(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("ArrivalAirport");
        if (!string.IsNullOrWhiteSpace(fieldValue)) return Result.Ok(fieldValue);

        var errorMessage = """
                           Bad Data Encountered in 'ArrivalAirport' Field

                           Flight Arrival Airport:
                            * Required
                            * Non-Empty Freetext
                                                    
                           """;
        return Result.Fail(errorMessage);
    }
}