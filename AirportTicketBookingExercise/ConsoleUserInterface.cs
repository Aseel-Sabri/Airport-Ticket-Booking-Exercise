using AirportTicketBookingExercise.Services;

namespace AirportTicketBookingExercise;

public class ConsoleUserInterface : IUserInterface
{
    private readonly IUserServices _userServices = new UserServices();

    public void Run()
    {
        while (!_userServices.Login())
        {
            Console.WriteLine();
        }

        Console.WriteLine("Logged In Successfully");
    }
}