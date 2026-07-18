using Microsoft.EntityFrameworkCore;
using OniBusExpress.Domain.Entities;
using OniBusExpress.Domain.Enums;
using OniBusExpress.Domain.Repositories.Reservation;

namespace OniBusExpress.Infrastructure.DataAccess.Repositories
{
    public class ReservationRepository : IReservationReadOnlyRepository, IReservationWriteOnlyRepository, IReservationUpdateOnlyRepository
    {
        private readonly OniBusExpressDbContext _dbContext;

        public ReservationRepository(OniBusExpressDbContext dbContext) => _dbContext = dbContext;

        public async Task Add(Reservation reservation) => await _dbContext.Reservations.AddAsync(reservation);

        public async Task<bool> ExistsActiveReservationForSeat(Guid tripId, int seatNumber)
        {
            return await _dbContext.Reservations.AnyAsync(reservation =>
                reservation.TripId == tripId &&
                reservation.SeatNumber == seatNumber &&
                reservation.Status == ReservationStatus.Active);
        }

        public async Task<bool> ExistsActiveReservationForPassenger(Guid tripId, Guid passengerId)
        {
            return await _dbContext.Reservations.AnyAsync(reservation =>
                reservation.TripId == tripId &&
                reservation.PassengerId == passengerId &&
                reservation.Status == ReservationStatus.Active);
        }

        public async Task<bool> ExistsReservationWithCode(string reservationCode)
        {
            return await _dbContext.Reservations.AnyAsync(reservation => reservation.ReservationCode == reservationCode);
        }

        async Task<Reservation?> IReservationReadOnlyRepository.GetByCode(string reservationCode)
        {
            return await _dbContext
                .Reservations
                .AsNoTracking()
                .Include(reservation => reservation.Passenger)
                .Include(reservation => reservation.Trip)
                    .ThenInclude(trip => trip.Route)
                .FirstOrDefaultAsync(reservation => reservation.ReservationCode == reservationCode);
        }
        async Task<Reservation?> IReservationUpdateOnlyRepository.GetByCode(string reservationCode)
        {
            return await _dbContext
                .Reservations
                .Include(reservation => reservation.Trip)
                .FirstOrDefaultAsync(reservation => reservation.ReservationCode == reservationCode);
        }

        public void Update(Reservation reservation) => _dbContext.Reservations.Update(reservation);

    }
}
