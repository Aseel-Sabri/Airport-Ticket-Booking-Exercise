using AirportTicketBookingExercise.Services;

namespace AirportTicketBookingExercise.UserInterface;

public class ManagerMenuDisplay : MenuDisplay
{
    private readonly IFlightServices _flightServices = new FlightServices();

    enum ManagerOperation
    {
        ViewBookings = 1,
        UploadFlights = 2,
        Exit = 3
    }

    public override void DisplayUserMenu()
    {
        DisplayMenu(ManagerOperation.Exit);
    }

    protected override void DisplayOptions()
    {
        Console.WriteLine("\nChoose Operation\n");
        Console.WriteLine("1. View all bookings");
        Console.WriteLine("2. Upload flights from CSV file");
        Console.WriteLine("3. Exit");
    }

    protected override void PerformOperation(Enum operation)
    {
        var managerOperation = (ManagerOperation)operation;
        switch (operation)
        {
            case ManagerOperation.ViewBookings:
            {
                _flightServices.ViewAllBookings();
                return;
            }
            case ManagerOperation.UploadFlights:
            {
                _flightServices.UploadFlights();
                return;
            }
        }
    }
}