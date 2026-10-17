using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComfySelf.Scheduling.Application.Models.DTOs;

namespace ComfySelf.Scheduling.Application.Clients.Abstractions
{
    public interface IPatientClient
    {
        Task<PatientDTO> GetPatientById (Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsPatient (Guid id, CancellationToken cancellationToken);
    }
}
