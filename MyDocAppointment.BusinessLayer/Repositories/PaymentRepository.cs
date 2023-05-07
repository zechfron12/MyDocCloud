using MyDocAppointment.BusinessLayer.Data;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.BusinessLayer.Repositories
{
    public class PaymentRepository : Repository<Payment>
    {
        public PaymentRepository(MyDocAppointmentDatabaseContext context) : base(context)
        {
        }
    }
}
