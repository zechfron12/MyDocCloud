using Microsoft.EntityFrameworkCore;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.BusinessLayer.Data
{
    public class MyDocAppointmentDatabaseContext : DbContext
    {

        public MyDocAppointmentDatabaseContext(DbContextOptions<MyDocAppointmentDatabaseContext> options) : base(options)
        {
            this.Database.EnsureCreated();

        }


        public DbSet<Hospital> Hospitals => Set<Hospital>();

        public DbSet<Appointment> Appointments => Set<Appointment>();

        public DbSet<Patient> Patients => Set<Patient>();

        public DbSet<Doctor> Doctors => Set<Doctor>();

        public DbSet<Medication> Medications => Set<Medication>();

        public DbSet<Prescription> Prescriptions => Set<Prescription>();
        public DbSet<MedicationDosagePrescription> MedicationDosagePrescriptions => Set<MedicationDosagePrescription>();
        public DbSet<MedicationDosageHistory> MedicationDosageHistories  => Set<MedicationDosageHistory>();

        public DbSet<Bill> Bills => Set<Bill>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //set null hospitalId on doctor when delete hospital
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Hospial)
                .WithMany(h => h.Doctors)
                .HasForeignKey(@"HospitalId")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .IsRequired(false);
            modelBuilder.Entity<Bill>()
                    .HasOne(b => b.Payment)
                    .WithOne(p => p.Bill)
                    .HasForeignKey<Payment>(p => p.BillId);

            base.OnModelCreating(modelBuilder);


        }
    }
}
