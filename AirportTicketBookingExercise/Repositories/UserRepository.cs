using AirportTicketBookingExercise.CsvOperations;
using AirportTicketBookingExercise.Models;
using FluentResults;

namespace AirportTicketBookingExercise.Repositories;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users;

    public UserRepository()
    {
        IDataManager dataManager = CsvDataManager.Instance;
        _users = dataManager.Users;
    }

    public Result<User> ValidateUserCredentials(string? username, string? password)
    {
        var userResult = GetUserByUsername(username);
        if (userResult.IsFailed)
        {
            return userResult.ToResult();
        }

        return IsValidPassword() ? Result.Ok(userResult.Value) : Result.Fail("Invalid Password");

        #region local function

        bool IsValidPassword()
        {
            // TODO: Hash Password
            return userResult.Value.Password == password;
        }

        #endregion
    }

    private Result<User> GetUserByUsername(string? username)
    {
        var user = _users
            .FirstOrDefault(user => user.Username == username);
        return user == null
            ? Result.Fail($"Invalid Username: No user with username '{username}' exists")
            : Result.Ok(user);
    }

    public Result<User> GetUserById(int userId)
    {
        var user = _users
            .FirstOrDefault(user => user.Id == userId);
        return user == null
            ? Result.Fail($"Invalid User ID: No user with id '{userId}' exists")
            : Result.Ok(user);
    }
}