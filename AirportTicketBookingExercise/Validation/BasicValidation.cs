namespace AirportTicketBookingExercise.Validation;

public static class BasicValidation
{
    public static bool IsPositiveDouble(string? input)
    {
        return double.TryParse(input, out var conversionOutput) && conversionOutput > 0;
    }

    public static bool IsPositiveInteger(string? input)
    {
        return int.TryParse(input, out var conversionOutput) && conversionOutput > 0;
    }
}