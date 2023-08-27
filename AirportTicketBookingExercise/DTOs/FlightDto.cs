using AirportTicketBookingExercise.Models;

namespace AirportTicketBookingExercise.DTOs;

public class FlightDto
{
    public double? Price { get; set; }

    public FieldOperator PriceOperator { get; set; } = FieldOperator.Equal;
    public string? DepartureCountry { get; set; }
    public string? DestinationCountry { get; set; }
    public DateTime? DepartureDate { get; set; }
    public FieldOperator DepartureDateOperator { get; set; } = FieldOperator.Equal;

    public string? DepartureAirport { get; set; }
    public string? ArrivalAirport { get; set; }
    public FlightClass.ClassType? Class { get; set; }


    public enum FieldOperator
    {
        Greater,
        Equal,
        Less
    }
}