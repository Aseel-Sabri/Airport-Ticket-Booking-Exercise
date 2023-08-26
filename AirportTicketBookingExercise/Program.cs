using AirportTicketBookingExercise.DataLoader;
using AirportTicketBookingExercise.UserInterface;

namespace AirportTicketBookingExercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDataLoader dataLoader = CsvDataLoader.Instance;
            var loaderResult = dataLoader.LoadData();
            if (loaderResult.IsFailed)
            {
                foreach (var error in loaderResult.Errors)
                {
                    Console.WriteLine(error.Message);
                }

                return;
            }

            IUserInterface userInterface = new ConsoleUserInterface();
            userInterface.Run();
        }
    }
}