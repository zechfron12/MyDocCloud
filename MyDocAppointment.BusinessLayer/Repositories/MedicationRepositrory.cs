using MyDocAppointment.BusinessLayer.Data;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.BusinessLayer.Repositories
{
    public class MedicationRepositrory : Repository<Medication>
    {
        public MedicationRepositrory(MyDocAppointmentDatabaseContext context) : base(context)
        {
        }
    }
}
