using ComfySelf.Scheduling.Application.Models.DTOs;

namespace ComfySelf.Scheduling.Application.Services.Abstractions
{
    public interface IAppointmentService
    {
        public Task CreateAppointment(CreateAppointmentDTO appointment, CancellationToken cancellationToken);
        public Task UpdateAppointment(UpdateAppointmentDTO appointment, CancellationToken cancellationToken);
        public Task DeleteAppointment(Guid appointmentId, CancellationToken cancellationToken);
        public Task<AppointmentDTO> GetAppointmentById(Guid appointmentId, CancellationToken cancellationToken);
        public Task<List<AppointmentDTO>> GetAllAppointments(CancellationToken cancellationToken);
        public Task<PatientDTO> GetPatientById(Guid patientId, CancellationToken ct);
        public Task<bool> ExistsPatient(Guid patientId, CancellationToken ct);
    }
}
