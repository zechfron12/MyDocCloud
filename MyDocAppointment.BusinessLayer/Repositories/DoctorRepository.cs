using Microsoft.EntityFrameworkCore;
using MyDocAppointment.BusinessLayer.Data;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.BusinessLayer.Repositories
{
    public class DoctorRepository : Repository<Doctor>
    {
        public DoctorRepository(MyDocAppointmentDatabaseContext context) : base(context)
        {
        }

        public override async Task<Doctor?> GetById(Guid id)
        {
            return await context.Doctors.Include(d => d.Appointments).FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
