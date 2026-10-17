using ComfySelf.Scheduling.Domain.Enums;

namespace ComfySelf.Scheduling.Domain.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
