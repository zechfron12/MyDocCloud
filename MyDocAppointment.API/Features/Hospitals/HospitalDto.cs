namespace MyDocAppointment.API.Features.Hospitals
{
    public class HospitalDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
