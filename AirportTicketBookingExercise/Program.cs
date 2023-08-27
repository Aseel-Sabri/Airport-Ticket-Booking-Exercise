using AirportTicketBookingExercise.CsvOperations;
using AirportTicketBookingExercise.UserInterface;

namespace AirportTicketBookingExercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDataManager dataManager = CsvDataManager.Instance;
            var loaderResult = dataManager.LoadData();
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

            var savingResult = dataManager.WriteData();
            if (savingResult.IsFailed)
            {
                foreach (var error in savingResult.Errors)
                {
                    Console.WriteLine(error.Message);
                }
            }
        }
    }
}