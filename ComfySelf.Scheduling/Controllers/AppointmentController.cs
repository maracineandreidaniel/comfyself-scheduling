using ComfySelf.Scheduling.Application.Models.DTOs;
using ComfySelf.Scheduling.Application.Services.Abstractions;
using ComfySelf.Scheduling.Domain;
using ComfySelf.Scheduling.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComfySelf.Scheduling.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService; 

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<AppointmentDTO>>> GetAll(
       CancellationToken cancellationToken)
        {
            var appointments = await _appointmentService.GetAllAppointments(
                cancellationToken);

            return Ok(appointments);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AppointmentDTO>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var appointment = await _appointmentService.GetAppointmentById(
                id,
                cancellationToken);

            if (appointment is null)
                return NotFound();

            return Ok(appointment);
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDTO>> Create(
            CreateAppointmentDTO dto,
            CancellationToken cancellationToken)
        {
            await _appointmentService.CreateAppointment(
                 dto,
                 cancellationToken);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            UpdateAppointmentDTO dto,
            CancellationToken cancellationToken)
        {
            await _appointmentService.UpdateAppointment(
                  dto,
                  cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _appointmentService.DeleteAppointment(
                 id,
                 cancellationToken);

            return NoContent();
        }

        [HttpGet("Patient/{patientId:guid}")]
        public async Task<ActionResult<PatientDTO>> GetPatientById(Guid patientId, CancellationToken ct)
        {
            var result = await _appointmentService.GetPatientById(patientId, ct);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("ExistsPatient/{patientId:guid}")]
        public async Task<ActionResult<PatientDTO>> ExistsPatient(Guid patientId, CancellationToken ct)
        {
            var result = await _appointmentService.ExistsPatient(patientId, ct);
            return Ok(result);
        }
    }
}
