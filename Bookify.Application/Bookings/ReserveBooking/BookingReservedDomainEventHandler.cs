using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Users;
using MediatR;

namespace Bookify.Application.Bookings.ReserveBooking;

internal sealed class BookingReservedDomainEventHandler : INotificationHandler<BookingReservedDomainEvent>
{
    private readonly IBookingRepository bookingRepository;
    private readonly IUserRepository userRepository;
    private readonly IEmailService emailService;

    public BookingReservedDomainEventHandler(
        IBookingRepository bookingRepository,
        IUserRepository userRepository,
        IEmailService emailService)
    {
        this.bookingRepository = bookingRepository;
        this.userRepository = userRepository;
        this.emailService = emailService;
    }

    public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByIdAsync(notification.BookingId, cancellationToken);

        if (booking is null)
        {
            return;
        }

        var user = await userRepository.GetByIdAsync(booking.UserId, cancellationToken);

        if (user is null)
        {
            return;
        }

        await emailService.SendAsync(
            user.Email,
            "Booking reserved!",
            "You have to confirm this booking");
    }
}
