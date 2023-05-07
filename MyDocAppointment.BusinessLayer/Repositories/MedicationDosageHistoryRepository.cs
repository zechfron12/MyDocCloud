using MyDocAppointment.BusinessLayer.Data;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.BusinessLayer.Repositories
{
    public class MedicationDosageHistoryRepository : Repository<MedicationDosageHistory>
    {
        public MedicationDosageHistoryRepository(MyDocAppointmentDatabaseContext context) : base(context)
        {
        }
    }
}
