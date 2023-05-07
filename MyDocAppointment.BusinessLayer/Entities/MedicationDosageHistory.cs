using ShelterManagement.Business.Helpers;

namespace MyDocAppointment.BusinessLayer.Entities
{
    public class MedicationDosageHistory : MedicationDosage
    {
        public MedicationDosageHistory(DateTime startDate, DateTime endDate, float quantity, float frequency) : base(startDate, endDate, quantity, frequency)
        {
        }

        public History? History { get; private set; }

        public Guid HistoryId { get; private set; }

        public Result RegisterMedicationInfoToHistory(History? history)
        {
            if(history == null)
            {
                return Result.Failure("History should not be null");
            }

            History = history;
            HistoryId = history.Id;
            return Result.Success();
        }

    }
}
