using MyDocAppointment.BusinessLayer.Data;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.BusinessLayer.Repositories
{
    public class HospitalRepository : Repository<Hospital>
    {
        public HospitalRepository(MyDocAppointmentDatabaseContext context) : base(context)
        {
        }
    }
}
