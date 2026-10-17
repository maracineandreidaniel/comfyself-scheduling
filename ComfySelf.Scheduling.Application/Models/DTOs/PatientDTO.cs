namespace ComfySelf.Scheduling.Application.Models.DTOs
{
    public sealed record PatientDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Birthday { get; set; }
    }
}
