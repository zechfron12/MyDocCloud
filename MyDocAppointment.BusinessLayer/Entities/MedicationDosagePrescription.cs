using ShelterManagement.Business.Helpers;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public class MedicationDosagePrescription : MedicationDosage
    {
        public MedicationDosagePrescription(DateTime startDate, DateTime endDate, float quantity, float frequency) : base(startDate, endDate, quantity, frequency)
        {
        }

        public Guid PrescriptionId { get; private set; }

        public Result RegisterMedicationInfoToPrescription(Prescription? prescription)
        {
            if(prescription == null)
            {
                return Result.Failure("Prescription should not be null");
            }

            PrescriptionId = prescription.Id;
            return Result.Success();
        }
    }
}
