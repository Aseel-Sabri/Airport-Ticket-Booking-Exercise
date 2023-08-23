using AirportTicketBookingExercise.Models;
using FluentResults;

namespace AirportTicketBookingExercise.Repositories;

public interface IUserRepository
{
    public Result<User> ValidateUserCredentials(string? username, string? password);
}