namespace AirportTicketBookingExercise.CsvOperations;

public static class ErrorMessage
{
    public static string GenerateFieldErrorMessage(string fieldName, string className, string details)
    {
        return $"""
                Bad Data Encountered in '{fieldName}' Field

                {className} {fieldName}:
                {details}
                                                    
                """;
    }
}