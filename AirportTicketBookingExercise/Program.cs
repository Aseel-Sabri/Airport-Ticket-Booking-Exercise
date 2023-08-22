namespace AirportTicketBookingExercise
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDataLoader dataLoader = CsvDataLoader.Instance;
            dataLoader.LoadData();
        }
    }
}