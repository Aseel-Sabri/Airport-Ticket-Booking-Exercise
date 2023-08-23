namespace AirportTicketBookingExercise.UserInterface;

public class ManagerMenuDisplay : MenuDisplay
{
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
        Console.WriteLine("1. Book a flight");
        Console.WriteLine("2. Upload flights from CSV file");
        Console.WriteLine("3. Exit");
    }

    // TODO: Implement Operations
    protected override void PerformOperation(Enum operation)
    {
        var managerOperation = (ManagerOperation)operation;
        switch (operation)
        {
            case ManagerOperation.ViewBookings:
            {
                return;
            }
            case ManagerOperation.UploadFlights:
            {
                return;
            }
        }
    }
}