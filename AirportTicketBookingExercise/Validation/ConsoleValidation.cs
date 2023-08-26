using FluentResults;
using static AirportTicketBookingExercise.Models.FlightClass;

namespace AirportTicketBookingExercise.Validation;

public static class ConsoleValidation
{
    public static Result ValidateNullablePrice(string? priceString)
    {
        if (string.IsNullOrWhiteSpace(priceString))
            return Result.Ok();

        return Result
            .OkIf(BasicValidation.IsPositiveDouble(priceString),
                "Invalid Double Input");
    }

    public static Result ValidateNullableDate(string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return Result.Ok();

        return Result
            .OkIf(DateTime.TryParse(dateString, out _),
                "Invalid Date Input: Date Format DD/MM/YYYY");
    }

    public static Result ValidateNullableClassType(string? classString)
    {
        if (string.IsNullOrWhiteSpace(classString))
            return Result.Ok();

        return Result
            .OkIf(ClassType.TryParse<ClassType>(classString, out _),
                $"Invalid Class Type, Valid Classes: {ClassType.EconomyClass}, {ClassType.BusinessClass} & {ClassType.FirstClass}");
    }

    public static Result ValidateClassType(string? classString)
    {
        if (Enum.TryParse<ClassType>(classString, out _))
            return Result.Ok();

        return Result.Fail(
            $"Invalid Class Type, Valid Classes: {ClassType.EconomyClass}, {ClassType.BusinessClass} & {ClassType.FirstClass}");
    }

    public static Result ValidateId(string? idString)
    {
        return Result
            .OkIf(BasicValidation.IsPositiveInteger(idString), "Invalid ID: ID Must Be A Positive Integer");
    }
}