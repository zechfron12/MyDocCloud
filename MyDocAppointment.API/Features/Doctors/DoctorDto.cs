namespace MyDocAppointment.API.Features.Doctors
{
    public class DoctorDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Specialization { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Title { get; set; }
        public string? Profession { get; set; }
        public string? Location { get; set; }
        public double Grade { get; set; }

        public int Reviews { get; set; }
    }
}
