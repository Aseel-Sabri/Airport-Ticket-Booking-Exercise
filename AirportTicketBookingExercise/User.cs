﻿namespace AirportTicketBookingExercise;

public class User
{
    public enum UserRole
    {
        Manager,
        Passenger
    }

    public int Id { get; init; }
    public required UserRole Role { get; set; }
    public required string UserName { get; set; }

    // TODO: Password setter should probably have some kind of authentication first
    public required string Password { get; set; }


    public override string ToString()
    {
        return $"""
                User ID: {Id}
                    Role: {Role}
                    UserName: {UserName}
                """;
    }
}