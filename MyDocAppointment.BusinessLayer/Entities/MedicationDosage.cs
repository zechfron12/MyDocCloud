using ShelterManagement.Business.Helpers;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public abstract class MedicationDosage
    {

        protected MedicationDosage(DateTime startDate, DateTime endDate, float quantity, float frequency)
        {
            Id = Guid.NewGuid();
            StartDate = startDate;
            EndDate = endDate;
            Quantity = quantity;
            Frequency = frequency;
        }

        public Guid Id { get; private set; }

        public Medication? Medication { get; private set; }

        public Guid MedicationId  { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        //nr unitati de medicament (o pastila)
        public float Quantity { get; private set; }

        //timplul la care se ia Quantity unitati in ore (o pastila la 12 ore)
        public float Frequency { get; private set; }
        
        public Result AddMedication(Medication? medication)
        {
            if (medication == null)
            {
                return Result.Failure("Medication should not be null");
            }
            Medication = medication;
            MedicationId = medication.Id;

            return Result.Success();
        }

    }
}
