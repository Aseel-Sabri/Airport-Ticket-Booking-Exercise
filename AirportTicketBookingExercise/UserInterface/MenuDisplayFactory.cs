using AirportTicketBookingExercise.Models;
using AirportTicketBookingExercise.Services;

namespace AirportTicketBookingExercise.UserInterface;

public static class MenuDisplayFactory
{
    public static MenuDisplay CreatePassengerMenuDisplay(User loggedUser, IFlightServices flightServices)
    {
        return new PassengerMenuDisplay(loggedUser, flightServices);
    }

    public static MenuDisplay CreateManagerMenuDisplay(IFlightServices flightServices)
    {
        return new ManagerMenuDisplay(flightServices);
    }
}