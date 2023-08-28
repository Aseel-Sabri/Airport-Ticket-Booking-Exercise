using AirportTicketBookingExercise.Models;
using FluentResults;

namespace AirportTicketBookingExercise.CsvOperations;

public interface IDataManager
{
    public List<User> Users { get; protected set; }
    public List<Flight> Flights { get; protected set; }
    public List<Booking> Bookings { get; protected set; }
    public List<FlightClass> FlightClasses { get; set; }

    public Result LoadData();

    Result LoadEntitiesIntoList<TEntity, TMapper>(List<TEntity> entityList, string? filePath = null)
        where TMapper : IEntityMapper<TEntity>, new();

    public Result WriteData();
}