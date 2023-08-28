namespace AirportTicketBookingExercise.Models;

public class User
{
    public enum UserRole
    {
        Manager = 1,
        Passenger = 2
    }

    public int Id { get; init; }
    public required UserRole Role { get; set; }
    public required string Username { get; set; }

    // TODO: Password setter should probably have some kind of authentication first
    public required string Password { get; set; }


    public override string ToString()
    {
        return $"""
                User ID: {Id}
                    Role: {Role}
                    Username: {Username}
                """;
    }
}