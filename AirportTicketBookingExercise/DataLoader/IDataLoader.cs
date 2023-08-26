using AirportTicketBookingExercise.Models;
using FluentResults;

namespace AirportTicketBookingExercise.DataLoader;

public interface IDataLoader
{
    public List<User> Users { get; protected set; }
    public List<Flight> Flights { get; protected set; }
    public List<Booking> Bookings { get; protected set; }
    public List<FlightClass> FlightClasses { get; set; }

    public Result LoadData();
}