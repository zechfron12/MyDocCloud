namespace MyDocAppointment.API.Features.Medications
{
    public class MedicationDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Stock { get; set; }
        public string? Unit { get; set; }
        public int Capacity { get; set; }
        public double Price { get; set; }
    }
}
