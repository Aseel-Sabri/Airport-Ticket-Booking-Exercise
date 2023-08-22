using AirportTicketBookingExercise.Models;

namespace AirportTicketBookingExercise;

public interface IDataLoader
{
    public List<User> Users { get; protected set; }
    public List<Flight> Flights { get; protected set; }
    public List<Booking> Bookings { get; protected set; }

    public void LoadData();
}