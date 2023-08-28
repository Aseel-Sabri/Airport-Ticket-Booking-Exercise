namespace AirportTicketBookingExercise.Models;

public class FlightClass
{
    public required int Id { get; set; }
    public required Flight Flight { get; set; }
    public required ClassType Type { get; set; }
    public required double Price { get; set; }
    public required int Capacity { get; set; }

    public int PassengerCount { get; set; }

    public static int MaxId { get; private set; } = 0;

    public int GetAvailableNumberOfSeats()
    {
        return Capacity - PassengerCount;
    }

    public override string ToString()
    {
        return $"""
                Flight: {Flight}
                Class: {Type}
                Price: {Price}
                Number of Free Seats: {GetAvailableNumberOfSeats()}
                
                """;
    }

    public enum ClassType
    {
        EconomyClass = 1,
        BusinessClass = 2,
        FirstClass = 3
    }
}