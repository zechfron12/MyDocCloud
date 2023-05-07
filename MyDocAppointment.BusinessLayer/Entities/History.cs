using ShelterManagement.Business.Helpers;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public class History
    {
        public History(DateTime startDate, DateTime endDate)
        {
            Id = Guid.NewGuid();
            StartDate = startDate;
            EndDate = endDate;
            MedicationDosageHistories = new List<MedicationDosageHistory>();

        }

        public Guid Id { get; private set; }

        public ICollection<MedicationDosageHistory> MedicationDosageHistories { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public Patient? Patient { get; private set; }

        public Guid PatientId { get; private set; }

        public bool IsStartDateValid()
        {
            return DateTime.Now < StartDate;
        }

        public Result AddPatientToHistory(Patient? patient)
        {
            if (patient == null)
            {
                return Result.Failure("Patient should not be null");
            }

            this.Patient = patient;
            PatientId = patient.Id;
            return Result.Success();
            
        }

        public Result AddMedications(List<MedicationDosageHistory> medicationsDosages)
        {
            if (!medicationsDosages.Any())
            {
                return Result.Failure("You must add at least a medication dosage");
            }

            medicationsDosages.ForEach(m =>
            {
                m.RegisterMedicationInfoToHistory(this);
                MedicationDosageHistories.Add(m);
            });

            return Result.Success();
        }

    }
}
