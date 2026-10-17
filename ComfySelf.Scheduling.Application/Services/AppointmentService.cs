using ComfySelf.Scheduling.Application.Clients.Abstractions;
using ComfySelf.Scheduling.Application.Models.DTOs;
using ComfySelf.Scheduling.Application.Repositories.Abstractions;
using ComfySelf.Scheduling.Application.Services.Abstractions;
using ComfySelf.Scheduling.Domain.Models;
using Microsoft.Extensions.Logging;

namespace ComfySelf.Scheduling.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientClient _patientClient;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(IAppointmentRepository appointmentRepository,
            IPatientClient patientClient,
            ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _patientClient = patientClient;
            _logger = logger;
        }

        public async Task CreateAppointment(CreateAppointmentDTO appointment, CancellationToken cancellationToken)
        {
            var appointmentToBeAdded = new Appointment
            {
                Id = Guid.NewGuid(),
                Description = appointment.Description,
                StartDate = appointment.StartDate,
                EndDate = appointment.EndDate,
                PatientId = appointment.PatientId
            };

            await _appointmentRepository.CreateAppointment(appointmentToBeAdded, cancellationToken);
        }

        public async Task DeleteAppointment(Guid appointmentId, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository
              .GetAppointmentById(appointmentId, cancellationToken);

            if (appointment is null)
            {
                throw new KeyNotFoundException(
                    $"Appointment with id '{appointmentId}' was not found.");
            }

            await _appointmentRepository.DeleteAppointment(appointmentId, cancellationToken);
        }

        public async Task<List<AppointmentDTO>> GetAllAppointments(CancellationToken cancellationToken)
        {
            var appointments = await _appointmentRepository.GetAllAppointments(cancellationToken);

            return appointments
           .Select(MapToDto)
           .ToList();
        }

        public async Task<AppointmentDTO> GetAppointmentById(Guid appointmentId, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository
                .GetAppointmentById(appointmentId, cancellationToken);

            if (appointment is null)
            {
                throw new KeyNotFoundException(
                    $"Appointment with id '{appointmentId}' was not found.");
            }

            return MapToDto(appointment);
        }

        public async Task UpdateAppointment(UpdateAppointmentDTO appointmentRequest, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(appointmentRequest.IdToUpdate, cancellationToken);

            if (appointment is null)
            {
                throw new KeyNotFoundException(
                    $"Appointment with id '{appointmentRequest.IdToUpdate}' was not found.");
            }

            appointment.Description = appointmentRequest.Description;
            appointment.StartDate = appointmentRequest.StartDate;
            appointment.EndDate = appointmentRequest.EndDate;
            appointment.PatientId = appointmentRequest.PatientId;

            await _appointmentRepository.UpdateAppointment(appointment, cancellationToken);
        }

        private AppointmentDTO MapToDto(Appointment appointment)
        {
            return new AppointmentDTO
            {
                Id = appointment.Id,
                Description = appointment.Description,
                StartDate = appointment.StartDate,
                EndDate = appointment.EndDate,
                PatientId = appointment.PatientId
            };

        }

        public async Task<PatientDTO> GetPatientById(Guid patientId, CancellationToken ct)
        {
            var patient = await _patientClient.GetPatientById(patientId, ct);

            if(patient == null)
            {
                _logger.LogInformation("There was no patient with that Id.");
                return null;
            }

            return patient;
        }

        public async Task<bool> ExistsPatient(Guid patientId, CancellationToken ct)
        {
            return await _patientClient.ExistsPatient(patientId,ct);
        }
    }
}
