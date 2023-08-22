namespace AirportTicketBookingExercise.Models;

public class Flight
{
    public required int Id { get; set; }
    public required string DepartureCountry { get; set; }
    public required string DestinationCountry { get; set; }
    public required DateTime DepartureDate { get; set; }
    public required string DepartureAirport { get; set; }
    public required string ArrivalAirport { get; set; }

    public static int MaxId { get; private set; } = 0;

    public override string ToString()
    {
        return $"""
                 Flight ID: {Id}
                      Departure Country: {DepartureCountry}
                      Destination Country: {DestinationCountry}
                      Departure Date: {DepartureDate}
                      Departure Airport: {DepartureAirport}
                      Arrival Airport: {ArrivalAirport}
                """;
    }
}