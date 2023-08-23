namespace AirportTicketBookingExercise.Models;

public class Booking
{
    public User Passenger { get; set; }
    public FlightClass FlightClass { get; set; }

    public string PassengerBookingToString()
    {
        return $"""
                {FlightClass.Flight},
                      Class: {FlightClass.Type}
                      Price: {FlightClass.Price}
                      
                """;
    }
}