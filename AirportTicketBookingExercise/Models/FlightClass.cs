namespace AirportTicketBookingExercise.Models;

public class FlightClass
{
    public required int Id { get; set; }
    public required Flight Flight { get; set; }
    public required ClassType Type { get; set; }
    public required double Price { get; set; }
    public required double Capacity { get; set; }
    public List<User> Passengers { get; set; } = new List<User>();

    public static int MaxId { get; private set; } = 0;

    public override string ToString()
    {
        return $"""
                ID: {Id}
                    Flight: {Flight.Id}
                    Type: {Type}
                    Price: {Price}
                    Capacity: {Capacity}
                """;
    }

    public enum ClassType
    {
        Economy = 1,
        Business = 2,
        FirstClass = 3
    }
}