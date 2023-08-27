using FluentResults;
using static AirportTicketBookingExercise.Models.FlightClass;

namespace AirportTicketBookingExercise.Validation;

public static class ConsoleValidation
{
    public static Result ValidateNullableComparablePrice(string? priceString)
    {
        if (string.IsNullOrWhiteSpace(priceString))
            return Result.Ok();

        var isValid = BasicValidation.IsPositiveDouble(priceString) ||
                      BasicValidation.IsPositiveDouble(priceString.Substring(1)); // in case an operator exists

        return Result
            .OkIf(isValid,
                "Invalid Double Input");
    }

    public static Result ValidateNullableComparableDate(string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return Result.Ok();

        var isValid = DateTime.TryParse(dateString, out _) ||
                      DateTime.TryParse(dateString.Substring(1), out _); // in case an operator exists


        return Result
            .OkIf(isValid,
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