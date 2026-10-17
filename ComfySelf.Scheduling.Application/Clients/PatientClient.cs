using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ComfySelf.Scheduling.Application.Clients.Abstractions;
using ComfySelf.Scheduling.Application.Models.DTOs;
using Microsoft.Extensions.Logging;

namespace ComfySelf.Scheduling.Application.Clients
{
    public sealed class PatientClient : IPatientClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PatientClient> _logger;

        public PatientClient(HttpClient httpClient, ILogger<PatientClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> ExistsPatient(Guid id, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"Patient/ExistsPatient/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken);

            return result;
        }

        public async Task<PatientDTO> GetPatientById(Guid id, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"Patient/{id}", cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var patient = await response.Content.ReadFromJsonAsync<PatientDTO>(cancellationToken);

            if (patient == null)
            {
                throw new InvalidOperationException("Patients API returned an empty response body.");
            }

            _logger.LogDebug($"Retrieved patient {id} from patient API.", id);

            return patient;
        }
    }
    }
