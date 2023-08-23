using System.Globalization;
using AirportTicketBookingExercise.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace AirportTicketBookingExercise;

public class CsvDataLoader : IDataLoader
{
    public List<User> Users { get; set; } = new List<User>();
    public List<Flight> Flights { get; set; } = new List<Flight>();

    public List<FlightClass> FlightClasses { get; set; } = new List<FlightClass>();
    public List<Booking> Bookings { get; set; } = new List<Booking>();

    private static CsvDataLoader? _instance;

    public static CsvDataLoader Instance
    {
        get { return _instance ??= new CsvDataLoader(); }
    }

    private CsvDataLoader()
    {
    }

    public void LoadData()
    {
        Users = LoadEntities<User>();
        Flights = LoadEntities<Flight>();
        FlightClasses = LoadEntities<FlightClass>();
        Bookings = LoadEntities<Booking>();
        UpdateFlightClassPassengerCount();
    }

    private void UpdateFlightClassPassengerCount()
    {
        FlightClasses.ForEach(flightClass =>
            flightClass.PassengerCount = Bookings.Count(booking => booking.FlightClass.Id == flightClass.Id));
    }

    private List<T> LoadEntities<T>()
    {
        // TODO: Handle Exceptions & Data Validation
        var fileName = $"Data/{typeof(T).Name}.csv";
        var filePath =
            Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                fileName);
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        if (typeof(T) == typeof(Booking))
        {
            csv.Context.RegisterClassMap<BookingMap>();
        }
        else if (typeof(T) == typeof(FlightClass))
        {
            csv.Context.RegisterClassMap<FlightClassMap>();
        }

        return csv.GetRecords<T>().ToList();
    }

    private class FlightClassMap : ClassMap<FlightClass>
    {
        public FlightClassMap()
        {
            Map(flightClass => flightClass.Id).Name("Id");
            Map(flightClass => flightClass.Capacity).Name("Capacity");
            Map(flightClass => flightClass.Price).Name("Price");
            Map(flightClass => flightClass.Type).Name("Type");
            var flights = CsvDataLoader.Instance.Flights;
            Map(flightClass => flightClass.Flight)
                .Convert(convertFromStringFunction: args =>
                    flights.FirstOrDefault(flight =>
                        flight.Id == int.Parse(args.Row.GetField("FlightId"))));
        }
    }

    private class BookingMap : ClassMap<Booking>
    {
        public BookingMap()
        {
            var flightClasses = CsvDataLoader.Instance.FlightClasses;
            Map(booking => booking.FlightClass)
                .Convert(convertFromStringFunction: args =>
                    flightClasses.FirstOrDefault(flight =>
                        flight.Id == int.Parse(args.Row.GetField("FlightClassId"))));

            var users = CsvDataLoader.Instance.Users;
            Map(booking => booking.Passenger)
                .Convert(convertFromStringFunction: args =>
                    users.FirstOrDefault(user =>
                        user.Id == int.Parse(args.Row.GetField("PassengerId"))));
        }
    }
}