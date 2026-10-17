using ComfySelf.Scheduling.Application.Repositories.Abstractions;
using ComfySelf.Scheduling.Domain.Models;
using ComfySelf.Scheduling.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComfySelf.Scheduling.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {

        private readonly SchedulingDbContext _dbContext;

        public AppointmentRepository(SchedulingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAppointment(Appointment appointment, CancellationToken cancellationToken)
        {
            await _dbContext.Appointments.AddAsync(appointment, cancellationToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAppointment(Guid appointmentId, CancellationToken cancellationToken)
        {
            var appointment = await _dbContext.Appointments.FirstOrDefaultAsync(p => p.Id == appointmentId, cancellationToken);

            if (appointment != null)
            {
                _dbContext.Appointments.Remove(appointment);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Appointment>> GetAllAppointments(CancellationToken cancellationToken)
        {
            var appointments = await _dbContext.Appointments.AsNoTracking().ToListAsync(cancellationToken);
            return appointments;
        }

        public async Task<Appointment> GetAppointmentById(Guid appointmentId, CancellationToken cancellationToken)
        {
            var appointment = await _dbContext.Appointments.FirstOrDefaultAsync(p => p.Id == appointmentId, cancellationToken);

            return appointment;
        }

        public async Task UpdateAppointment(Appointment appointmentRequest, CancellationToken cancellationToken)
        {
            var appointment = await _dbContext.Appointments.FirstOrDefaultAsync(p => p.Id == appointmentRequest.Id, cancellationToken);

            _dbContext.Update(appointment);
            _dbContext.SaveChanges();
        }
    }
}
