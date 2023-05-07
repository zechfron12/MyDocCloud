using Microsoft.EntityFrameworkCore;
using MyDocAppointment.BusinessLayer.Data;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.BusinessLayer.Repositories
{
    public class AppointmentRepository : Repository<Appointment>
    {
        public AppointmentRepository(MyDocAppointmentDatabaseContext context) : base(context)
        {
        }

       
        public override async Task<IReadOnlyCollection<Appointment>> GetAll()
        {
            return await context.Appointments.Include(a => a.Doctor).Include(a => a.Patient).ToListAsync();
        }
    }
}
