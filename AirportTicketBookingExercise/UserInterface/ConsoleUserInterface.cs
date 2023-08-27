using AirportTicketBookingExercise.Models;
using AirportTicketBookingExercise.Services;

namespace AirportTicketBookingExercise.UserInterface;

public class ConsoleUserInterface : IUserInterface
{
    private readonly IUserServices _userServices;
    private readonly IFlightServices _flightServices;
    private User _loggedUser;

    public ConsoleUserInterface(IUserServices userServices, IFlightServices flightServices)
    {
        _userServices = userServices;
        _flightServices = flightServices;
    }

    public void Run()
    {
        Login();
        DisplayMenu();
    }

    private void Login()
    {
        var userLoginResult = _userServices.Login();
        while (userLoginResult.IsFailed)
        {
            userLoginResult = _userServices.Login();
            Console.WriteLine();
        }

        Console.WriteLine("Logged In Successfully");

        _loggedUser = userLoginResult.Value;
    }

    private void DisplayMenu()
    {
        MenuDisplay menuDisplay;
        if (_loggedUser.Role == User.UserRole.Manager)
        {
            menuDisplay = MenuDisplayFactory.CreateManagerMenuDisplay(_flightServices);
        }
        else
        {
            menuDisplay = MenuDisplayFactory.CreatePassengerMenuDisplay(_loggedUser, _flightServices);
        }

        menuDisplay.DisplayUserMenu();
    }
}