using MyDocAppointment.BusinessLayer.Data;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.BusinessLayer.Repositories
{
    public class HistoryRepository : Repository<History>
    {
        public HistoryRepository(MyDocAppointmentDatabaseContext context) : base(context)
        {
        }
    }
}
