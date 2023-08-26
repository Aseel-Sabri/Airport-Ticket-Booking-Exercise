using CsvHelper;
using FluentResults;

namespace AirportTicketBookingExercise.DataLoader;

public interface IEntityMapper<TEntity>
{
    Result<TEntity> GetEntity(IReaderRow csvReader, List<TEntity> entities);
}