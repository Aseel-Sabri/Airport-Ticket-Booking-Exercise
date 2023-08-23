using AirportTicketBookingExercise.Services;
using FluentResults;

namespace AirportTicketBookingExercise.UserInterface;

public abstract class MenuDisplay
{
    public abstract void DisplayUserMenu();

    protected abstract void DisplayOptions();

    protected abstract void PerformOperation(Enum operation);


    protected void DisplayMenu<TOperationEnum>(TOperationEnum exitOp) where TOperationEnum : struct, Enum
    {
        Result<TOperationEnum> opResult;
        do
        {
            DisplayOptions();
            opResult = ReadUserOption<TOperationEnum>();
            if (opResult.IsFailed)
            {
                Console.WriteLine(opResult.Errors.First().Message);
                continue;
            }

            PerformOperation(opResult.ValueOrDefault);
        } while (!Equals(opResult.ValueOrDefault, exitOp));
    }

    private Result<TOperationEnum> ReadUserOption<TOperationEnum>() where TOperationEnum : struct, Enum
    {
        string? input = Console.ReadLine();
        Console.WriteLine();
        if (IsValidOperation())
        {
            Enum.TryParse<TOperationEnum>(input, out TOperationEnum op);
            return Result.Ok(op);
        }

        return Result.Fail($"Operation must be an integer within the specified operations");

        #region local function

        bool IsValidOperation()
        {
            return !string.IsNullOrEmpty(input)
                   && Enum.TryParse<TOperationEnum>(input, out TOperationEnum op)
                   && Enum.IsDefined<TOperationEnum>(op);
        }

        #endregion
    }
}