using AirportTicketBookingExercise.Models;
using AirportTicketBookingExercise.Services;

namespace AirportTicketBookingExercise.UserInterface;

public class ConsoleUserInterface : IUserInterface
{
    private readonly IUserServices _userServices = new UserServices();
    private User _loggedUser;

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
            menuDisplay = new ManagerMenuDisplay();
        }
        else
        {
            menuDisplay = new PassengerMenuDisplay(_loggedUser);
        }

        menuDisplay.DisplayUserMenu();
    }
}