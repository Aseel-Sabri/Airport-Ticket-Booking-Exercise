using AirportTicketBookingExercise.CsvOperations;
using AirportTicketBookingExercise.Repositories;
using AirportTicketBookingExercise.Services;
using AirportTicketBookingExercise.UserInterface;
using Microsoft.Extensions.DependencyInjection;

namespace AirportTicketBookingExercise;

public class Startup
{
    public IServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
            .AddSingleton<IDataManager, CsvDataManager>()
            .AddSingleton<IUserServices, UserServices>()
            .AddSingleton<IFlightServices, FlightServices>()
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IFlightRepository, FlightRepository>()
            .AddSingleton<IUserInterface, ConsoleUserInterface>()
            .BuildServiceProvider();
    }
}