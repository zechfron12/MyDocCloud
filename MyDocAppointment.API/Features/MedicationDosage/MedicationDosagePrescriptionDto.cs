namespace MyDocAppointment.API.Features.MedicationDosage
{
    public class MedicationDosagePrescriptionDto
    {
        public Guid MedicationId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public float Quantity { get; set; }

        public float Frequency { get; set; }
    }
}
