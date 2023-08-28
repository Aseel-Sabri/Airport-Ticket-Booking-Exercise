using CsvHelper;
using FluentResults;

namespace AirportTicketBookingExercise.CsvOperations;

public interface IEntityMapper<TEntity>
{
    Result<TEntity> GetEntity(IReaderRow csvReader, List<TEntity> entities);
}