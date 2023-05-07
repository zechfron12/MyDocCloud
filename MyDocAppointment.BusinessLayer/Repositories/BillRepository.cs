using Microsoft.EntityFrameworkCore;
using MyDocAppointment.BusinessLayer.Data;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.BusinessLayer.Repositories
{
    public class BillRepository : Repository<Bill>
    {
        public BillRepository(MyDocAppointmentDatabaseContext context) : base(context)
        {
        }

        public override async Task<Bill?> GetById(Guid id)
        {
            return await context.Bills.Include(b => b.Payment).Include(b => b.Medications).FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
