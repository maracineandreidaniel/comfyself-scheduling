using ComfySelf.Scheduling.Domain.Models;

namespace ComfySelf.Scheduling.Application.Repositories.Abstractions
{
    public interface IAppointmentRepository
    {
        public Task CreateAppointment(Appointment appointment, CancellationToken cancellationToken);
        public Task UpdateAppointment(Appointment appointment, CancellationToken cancellationToken);
        public Task DeleteAppointment(Guid appointmentId, CancellationToken cancellationToken);
        public Task<Appointment> GetAppointmentById(Guid appointmentId, CancellationToken cancellationToken);
        public Task<List<Appointment>> GetAllAppointments(CancellationToken cancellationToken);
    }
}
