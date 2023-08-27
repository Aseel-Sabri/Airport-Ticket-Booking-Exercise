using System.Globalization;
using AirportTicketBookingExercise.Models;
using CsvHelper;
using CsvHelper.Configuration;
using FluentResults;

namespace AirportTicketBookingExercise.CsvOperations;

public class CsvDataManager : IDataManager
{
    public List<User> Users { get; set; } = new();
    public List<Flight> Flights { get; set; } = new();
    public List<FlightClass> FlightClasses { get; set; } = new();
    public List<Booking> Bookings { get; set; } = new();

    private static CsvDataManager? _instance;

    public static CsvDataManager Instance
    {
        get { return _instance ??= new CsvDataManager(); }
    }

    public CsvDataManager()
    {
        _instance = this;
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

        UpdateFlightClassPassengerCount();

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
            Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                fileName);
        return filePath;
    }

    public Result LoadEntitiesIntoList<TEntity, TMapper>(List<TEntity> entityList, string? filePath = null)
        where TMapper : IEntityMapper<TEntity>, new()
    {
        if (entityList is null)
            return Result.Fail("Null List Provided");

        filePath ??= GetDefaultFilePath<TEntity>();

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


    public Result SaveEntities<TEntity, TEntityMap>(string filePath, List<TEntity> entityList)
        where TEntityMap : ClassMap<TEntity>
    {
        try
        {
            using var writer = new StreamWriter(filePath);
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            if (typeof(TEntityMap) != typeof(ClassMap<TEntity>))
            {
                csvWriter.Context.RegisterClassMap<TEntityMap>();
            }

            csvWriter.WriteHeader<TEntity>();
            csvWriter.NextRecord();
            csvWriter.WriteRecords(entityList);
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }

        return Result.Ok();
    }


    public Result WriteData()
    {
        var userSavingResult = SaveEntities<User, ClassMap<User>>(GetDefaultFilePath<User>(), Users);
        var flightSavingResult =
            SaveEntities<Flight, ClassMap<Flight>>(GetDefaultFilePath<Flight>(), Flights);
        var flightClassSavingResult =
            SaveEntities<FlightClass, FlightClassMapper.FlightClassMap>(GetDefaultFilePath<FlightClass>(),
                FlightClasses);
        var bookingSavingResult =
            SaveEntities<Booking, BookingMapper.BookingMap>(GetDefaultFilePath<Booking>(), Bookings);

        return Result.Merge(userSavingResult, flightSavingResult, flightClassSavingResult, bookingSavingResult);
    }
}