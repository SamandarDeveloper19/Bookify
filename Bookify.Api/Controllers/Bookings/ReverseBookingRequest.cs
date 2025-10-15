namespace Bookify.Api.Controllers.Bookings;

public sealed record ReverseBookingRequest(
    Guid ApartmentId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate);
