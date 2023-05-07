using ShelterManagement.Business.Helpers;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public class Prescription
    {
        public Prescription()
        {
            Id = Guid.NewGuid();
            MedicationDosagePrescriptions = new List<MedicationDosagePrescription>();
        }
        public Guid Id { get; private set; }

        public Doctor? Doctor { get; private set; }

        public Guid DoctorId { get; private set; }

        public Patient? Patient { get; private set; }

        public Guid PatientId { get; private set; }

        public ICollection<MedicationDosagePrescription> MedicationDosagePrescriptions { get; private set; }


        public Result AddDoctorToPrescription(Doctor? doctor)
        {
            if (doctor == null)
            {
                return Result.Failure("Doctor should not be null");
            }

            Doctor = doctor;
            DoctorId = doctor.Id;
            return Result.Success();
        }
        public Result AddPatientToPrescription(Patient? patient)
        {
            if (patient == null)
            {
                return Result.Failure("Patient should not be null");
            }

            Patient = patient;
            PatientId = patient.Id;
            return Result.Success();
        }
        public Result AddMedications(List<MedicationDosagePrescription> medicationsDosages)
        {
            if (!medicationsDosages.Any())
            {
                return Result.Failure("You must add at least one medication dosage");
            }
            medicationsDosages.ForEach(m =>
            {
                m.RegisterMedicationInfoToPrescription(this);
                MedicationDosagePrescriptions.Add(m);
            });
            return Result.Success();
        }
    }
}
