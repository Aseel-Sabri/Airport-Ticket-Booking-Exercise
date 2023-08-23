﻿using AirportTicketBookingExercise.DTOs;
using AirportTicketBookingExercise.Models;
using FluentResults;

namespace AirportTicketBookingExercise.Repositories;

public interface IFlightRepository
{
    IEnumerable<FlightClass> GetAvailableFilteredFlights(FlightDto flightDto, int passengerId);
    Result BookFlight(int flightId, FlightClass.ClassType flightClass, int passengerId);
}