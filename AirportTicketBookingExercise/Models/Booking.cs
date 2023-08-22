namespace AirportTicketBookingExercise.Models;

public class Booking
{
    public User Passenger { get; set; }
    public FlightClass FlightClass { get; set; }

    public override string ToString()
    {
        return $"""
                Passenger:
                    {Passenger.Id}
                Flight:
                    {FlightClass.Flight.Id},
                Class:
                    {FlightClass.Type}    
                """;
    }
}