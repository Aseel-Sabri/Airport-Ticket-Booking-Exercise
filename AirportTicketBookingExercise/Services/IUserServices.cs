using AirportTicketBookingExercise.Models;
using FluentResults;

namespace AirportTicketBookingExercise.Services;

public interface IUserServices
{
    Result<User> Login();
}