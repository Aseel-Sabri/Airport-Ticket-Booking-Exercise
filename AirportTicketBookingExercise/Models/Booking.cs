namespace AirportTicketBookingExercise.Models;

public class Booking
{
    public User Passenger { get; set; }
    public Flight Flight { get; set; }
    public FlightClass Class { get; set; }

    public override string ToString()
    {
        return $"""
                Flight:
                {Flight},
                Passenger:
                {Passenger}
                """;
    }
}