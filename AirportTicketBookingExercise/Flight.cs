namespace AirportTicketBookingExercise;

public class Flight
{
    private int _id;

    public int Id
    {
        get => _id;
        private set
        {
            _id = value;
            MaxId = int.Max(MaxId, _id);
        }
    }

    public required double Price { get; set; }
    public required string DepartureCountry { get; set; }
    public required string DestinationCountry { get; set; }
    public required DateTime DepartureDate { get; set; }
    public required string DepartureAirport { get; set; }
    public required string ArrivalAirport { get; set; }
    public required int EconomyClassCapacity { get; set; }
    public required int BusinessClassCapacity { get; set; }
    public required int FirstClassCapacity { get; set; }
    public List<int> PassengerIds { get; set; }
    
    public static int MaxId { get; private set; } = 0;
    
    public Flight()
    {
        Id = ++MaxId;
    }

    public override string ToString()
    {
        return $"""
                 Flight ID: {Id}
                      Price: {Price}
                      Departure Country: {DepartureCountry}
                      Destination Country: {DestinationCountry}
                      Departure Date: {DepartureDate}
                      Departure Airport: {DepartureAirport}
                      Arrival Airport: {ArrivalAirport}
                      Economy Class Capacity: {EconomyClassCapacity}
                      Business Class Capacity: {BusinessClassCapacity}
                      First Class Capacity: {FirstClassCapacity}
                """;
    }
}