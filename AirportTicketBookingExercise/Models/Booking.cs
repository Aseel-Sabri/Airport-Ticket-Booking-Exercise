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

    public string ManagerBookingToString()
    {
        return $"""
                 Passenger:
                      ID: {Passenger.Id}
                      Username: {Passenger.Username}
                {FlightClass.Flight},
                      Class: {FlightClass.Type}
                      Price: {FlightClass.Price}
                
                """;
    }
}