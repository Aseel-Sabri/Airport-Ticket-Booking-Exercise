using System.Globalization;
using AirportTicketBookingExercise.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace AirportTicketBookingExercise;

public class CsvDataLoader : IDataLoader
{
    public List<User> Users { get; set; } = new List<User>();
    public List<Flight> Flights { get; set; } = new List<Flight>();
    public List<Booking> Bookings { get; set; } = new List<Booking>();

    private static CsvDataLoader? _instance = null;

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
        Bookings = LoadEntities<Booking>();
        FillFlightPassengers();
    }

    private void FillFlightPassengers()
    {
        Flights.ForEach(flight =>
        {
            var bookings = Bookings.Where(booking => booking.Flight.Id == flight.Id).ToList();
            flight.PassengerIds = bookings.Select(booking => booking.Passenger.Id).ToList();
        });
    }

    private List<T> LoadEntities<T>()
    {
        // TODO: Handle Exceptions & Data Validation
        var fileName = $"Data/{typeof(T).Name}s.csv";
        var filePath =
            Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                fileName);
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Context.RegisterClassMap<BookingMap>();
        return csv.GetRecords<T>().ToList();
    }

    private class BookingMap : ClassMap<Booking>
    {
        public BookingMap()
        {
            var flights = CsvDataLoader.Instance.Flights;
            Map(booking => booking.Flight)
                .Convert(convertFromStringFunction: args =>
                    flights.FirstOrDefault(flight =>
                        flight.Id == int.Parse(args.Row.GetField("FlightId"))));

            var users = CsvDataLoader.Instance.Users;
            Map(booking => booking.Passenger)
                .Convert(convertFromStringFunction: args =>
                    users.FirstOrDefault(flight =>
                        flight.Id == int.Parse(args.Row.GetField("PassengerId"))));
        }
    }
}