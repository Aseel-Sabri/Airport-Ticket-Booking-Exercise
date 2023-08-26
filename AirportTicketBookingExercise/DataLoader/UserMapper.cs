using AirportTicketBookingExercise.Models;
using AirportTicketBookingExercise.Validation;
using CsvHelper;
using FluentResults;

namespace AirportTicketBookingExercise.DataLoader;

public class UserMapper : IEntityMapper<User>
{
    public Result<User> GetEntity(IReaderRow csvReader, List<User> usersList)
    {
        var idResult = GetUserId(csvReader);
        var usernameResult = GetUsername(csvReader);
        var passwordResult = GetUserPassword(csvReader);
        var roleResult = GetUserRole(csvReader);
        var userResult = Result.Merge(idResult, usernameResult, passwordResult, roleResult);
        if (userResult.IsFailed)
            return userResult;

        var user = new User()
        {
            Id = idResult.Value,
            Username = usernameResult.Value,
            Password = passwordResult.Value,
            Role = roleResult.Value
        };
        var userUniquenessResult = CsvValidation.ValidateUserUniqueness(user, usersList);
        return userUniquenessResult.IsFailed ? userUniquenessResult : Result.Ok(user);
    }

    private Result<int> GetUserId(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("Id");
        if (CsvValidation.IsPositiveInteger(fieldValue))
        {
            var id = int.Parse(fieldValue!);
            return Result.Ok(id);
        }

        var errorDetails = """
                             * Required
                             * Positive Integer
                             * Unique
                           """;

        return Result.Fail(ErrorMessage.GenerateFieldErrorMessage("Id", "User", errorDetails));
    }

    private Result<string> GetUsername(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("Username");
        if (string.IsNullOrWhiteSpace(fieldValue))
        {
            var errorDetails = """
                                * Required
                                * Non-Empty Freetext
                                * Unique
                               """;
            return Result.Fail(ErrorMessage.GenerateFieldErrorMessage("Username", "User", errorDetails));
        }

        var username = fieldValue!.Trim();
        return Result.Ok(username);
    }

    private Result<string> GetUserPassword(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("Password");
        if (string.IsNullOrWhiteSpace(fieldValue))
        {
            var errorDetails = """
                                * Required
                                * Non-Empty Freetext
                               """;
            return Result.Fail(ErrorMessage.GenerateFieldErrorMessage("Password", "User", errorDetails));
        }

        var password = fieldValue!.Trim();
        return Result.Ok(password);
    }

    private Result<User.UserRole> GetUserRole(IReaderRow csvReader)
    {
        var fieldValue = csvReader.GetField<string>("Role");

        var errorDetails = $"""
                             * Required
                             * Value in ( {User.UserRole.Passenger}, {User.UserRole.Manager} )
                            """;

        return Enum.TryParse(fieldValue, out User.UserRole userRole)
            ? Result.Ok(userRole)
            : Result.Fail(ErrorMessage.GenerateFieldErrorMessage("Role", "User", errorDetails));
        ;
    }
}