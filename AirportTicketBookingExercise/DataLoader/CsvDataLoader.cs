using System.Globalization;
using AirportTicketBookingExercise.Models;
using CsvHelper;
using FluentResults;

namespace AirportTicketBookingExercise.DataLoader;

public class CsvDataLoader : IDataLoader
{
    public List<User> Users { get; set; } = new();
    public List<Flight> Flights { get; set; } = new();
    public List<FlightClass> FlightClasses { get; set; } = new();
    public List<Booking> Bookings { get; set; } = new();

    private static CsvDataLoader? _instance;

    public static CsvDataLoader Instance
    {
        get { return _instance ??= new CsvDataLoader(); }
    }

    private CsvDataLoader()
    {
    }

    public Result LoadData()
    {
        var usersResult = LoadEntitiesIntoList<User, UserMapper>(Users);
        if (usersResult.IsFailed)
            return usersResult;

        var flightsResult = LoadEntitiesIntoList<Flight, FlightMapper>(Flights);
        if (flightsResult.IsFailed)
            return flightsResult;

        var flightClassesResult = LoadEntitiesIntoList<FlightClass, FlightClassMapper>(FlightClasses);
        if (flightClassesResult.IsFailed)
            return flightClassesResult;

        var bookingResult = LoadEntitiesIntoList<Booking, BookingMapper>(Bookings);
        if (bookingResult.IsFailed)
            return bookingResult;

        return Result.Ok();
    }

    private void UpdateFlightClassPassengerCount()
    {
        FlightClasses.ForEach(flightClass =>
            flightClass.PassengerCount = Bookings.Count(booking => booking.FlightClass.Id == flightClass.Id));
    }

    private string GetDefaultFilePath<T>()
    {
        var fileName = $"Data/{typeof(T).Name}.csv";
        var filePath =
            Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                fileName);
        return filePath;
    }

    private Result LoadEntitiesIntoList<TEntity, TMapper>(List<TEntity> entityList, string? filePath = null)
        where TMapper : IEntityMapper<TEntity>, new()
    {
        if (entityList is null)
            return Result.Fail("Null List Provided");

        if (filePath is null)
            filePath = GetDefaultFilePath<TEntity>();

        try
        {
            using var streamReader = new StreamReader(filePath);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csvReader.Read();
            csvReader.ReadHeader();
            var loadingResult = GetEntities(csvReader);
            return loadingResult.IsFailed ? loadingResult : Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail($"Could Not Load {typeof(TEntity).Name} Data:{Environment.NewLine} {e.Message}");
        }

        #region local function

        Result GetEntities(CsvReader csvReader)
        {
            var mapper = new TMapper();
            var dataCount = 0;
            while (csvReader.Read())
            {
                var entityResult = mapper.GetEntity(csvReader, entityList);
                if (entityResult.IsFailed)
                {
                    var result =
                        Result.Fail(
                            $"Data Loading Aborted, Bad Data Encountered While Reading {typeof(TEntity).Name} Data, At Row {dataCount} {Environment.NewLine}");
                    return Result.Merge(result, entityResult.ToResult());
                }

                entityList.Add(entityResult.Value);
                dataCount++;
            }

            return Result.Ok();
        }

        #endregion
    }
}