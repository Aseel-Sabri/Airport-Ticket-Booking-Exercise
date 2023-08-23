namespace AirportTicketBookingExercise.UserInterface;

public class PassengerMenuDisplay : MenuDisplay
{
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
                return;
            }
            case PassengerOperation.ViewBookings:
            {
                return;
            }
            case PassengerOperation.EditBooking:
            {
                return;
            }
            case PassengerOperation.CancelBooking:
            {
                return;
            }
            case PassengerOperation.SearchAvailableFlight:
            {
                return;
            }
        }
    }
}