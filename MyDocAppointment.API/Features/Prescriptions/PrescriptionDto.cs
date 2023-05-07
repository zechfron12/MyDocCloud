using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.API.Features.Prescriptions
{
    public class PrescriptionDto
    {
        public Guid Id { get; set; }  
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }

        public ICollection<MedicationDosagePrescription>? MedicationDosagePrescriptions { get; set; }
    }
}
