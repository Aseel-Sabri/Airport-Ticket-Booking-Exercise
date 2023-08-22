using FluentResults;

namespace AirportTicketBookingExercise.Repositories;

public interface IUserRepository
{
    Result AreValidUserCredentials(string? username, string? password);
}