using AirportTicketBookingExercise.Models;
using AirportTicketBookingExercise.Services;

namespace AirportTicketBookingExercise.UserInterface;

public class PassengerMenuDisplay : MenuDisplay
{
    private readonly IUserServices _userServices = new UserServices();
    private readonly IFlightServices _flightServices = new FlightServices();
    private readonly User _loggedUser;

    public PassengerMenuDisplay(User loggedUser)
    {
        _loggedUser = loggedUser;
    }

    enum PassengerOperation
    {
        BookFlight = 1,
        ViewBookings = 2,
        EditBooking = 3,
        CancelBooking = 4,
        SearchAvailableFlight = 5,
        Exit = 6
    }

    public override void DisplayUserMenu()
    {
        base.DisplayMenu<PassengerOperation>(PassengerOperation.Exit);
    }

    protected override void DisplayOptions()
    {
        Console.WriteLine("\nChoose Operation\n");
        Console.WriteLine("1. Book a flight");
        Console.WriteLine("2. View my bookings");
        Console.WriteLine("3. Edit a booking");
        Console.WriteLine("4. Cancel a booking");
        Console.WriteLine("5. Search for flights available for booking");
        Console.WriteLine("6. Exit");
    }

    protected override void PerformOperation(Enum operation)
    {
        var managerOperation = (PassengerOperation)operation;
        switch (operation)
        {
            case PassengerOperation.BookFlight:
            {
                _flightServices.BookFlight(_loggedUser.Id);
                return;
            }
            case PassengerOperation.ViewBookings:
            {
                _userServices.ViewPassengerBookings(_loggedUser.Id);
                return;
            }
            case PassengerOperation.EditBooking:
            {
                _flightServices.EditBooking(_loggedUser.Id);
                return;
            }
            case PassengerOperation.CancelBooking:
            {
                return;
            }
            case PassengerOperation.SearchAvailableFlight:
            {
                _flightServices.SearchAvailableFlightsForBooking(_loggedUser.Id);
                return;
            }
        }
    }
}