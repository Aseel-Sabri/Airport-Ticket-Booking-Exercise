using AirportTicketBookingExercise.Repositories;

namespace AirportTicketBookingExercise.Services;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository = new UserRepository();

    public bool Login()
    {
        GetUserCredentials(out var username, out var password);
        var validationResult = _userRepository.AreValidUserCredentials(username, password);
        if (validationResult.IsFailed)
        {
            Console.WriteLine(validationResult.Errors.First().Message);
            return false;
        }

        return true;
    }

    private void GetUserCredentials(out string? username, out string? password)
    {
        Console.WriteLine("Login");

        Console.Write("Username: ");
        username = Console.ReadLine();

        Console.Write("Password: ");

        password = null;
        var key = Console.ReadKey(true);
        while (key.Key != ConsoleKey.Enter)
        {
            password += key.KeyChar;
            key = Console.ReadKey(true);
        }

        Console.WriteLine();
        Console.WriteLine();
    }
}