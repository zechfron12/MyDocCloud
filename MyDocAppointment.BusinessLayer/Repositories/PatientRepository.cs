using Microsoft.EntityFrameworkCore;
using MyDocAppointment.BusinessLayer.Data;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.BusinessLayer.Repositories
{
    public class PatientRepository : Repository<Patient>
    {
        public PatientRepository(MyDocAppointmentDatabaseContext context) : base(context)
        {
        }

        public override async Task<Patient?> GetById(Guid id)
        {
            return await context.Patients.Include(d => d.Appointments).FirstOrDefaultAsync(d => d.Id == id);
        }
    }
}
