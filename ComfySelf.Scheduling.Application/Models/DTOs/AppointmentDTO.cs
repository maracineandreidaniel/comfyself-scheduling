using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComfySelf.Scheduling.Domain.Enums;

namespace ComfySelf.Scheduling.Application.Models.DTOs
{
    public class AppointmentDTO
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
