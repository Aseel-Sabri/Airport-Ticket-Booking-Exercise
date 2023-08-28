using AirportTicketBookingExercise.CsvOperations;
using AirportTicketBookingExercise.UserInterface;
using Microsoft.Extensions.DependencyInjection;

namespace AirportTicketBookingExercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var startup = new Startup();
            var serviceProvider = startup.ConfigureServices();

            var dataManager = serviceProvider.GetRequiredService<IDataManager>();
            if (!LoadData(dataManager)) return;

            var userInterface = serviceProvider.GetRequiredService<IUserInterface>();
            userInterface.Run();

            SaveData(dataManager);
        }

        private static void SaveData(IDataManager dataManager)
        {
            var savingResult = dataManager.WriteData();
            if (savingResult.IsFailed)
            {
                foreach (var error in savingResult.Errors)
                {
                    Console.WriteLine(error.Message);
                }
            }
        }

        private static bool LoadData(IDataManager dataManager)
        {
            var loaderResult = dataManager.LoadData();
            if (loaderResult.IsFailed)
            {
                foreach (var error in loaderResult.Errors)
                {
                    Console.WriteLine(error.Message);
                }

                return false;
            }

            return true;
        }
    }
}