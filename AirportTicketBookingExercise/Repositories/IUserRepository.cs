using AirportTicketBookingExercise.Models;
using FluentResults;

namespace AirportTicketBookingExercise.Repositories;

public interface IUserRepository
{
    Result<User> ValidateUserCredentials(string? username, string? password);

    Result<User> GetUserById(int userId);
}