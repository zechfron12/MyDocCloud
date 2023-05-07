using Microsoft.Extensions.DependencyInjection;
using MyDocAppointment.BusinessLayer.Data;

namespace MyDocAppointment.Tests.ApiTests
{
    public class BaseIntegrationTests<T> : IClassFixture<CustomWebApplicationFactory<Program>> where T : class
    {
        protected HttpClient  HttpClient { get;private set; }
        protected CustomWebApplicationFactory<Program> Factory { get; private set; } 

        protected BaseIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            Factory = factory;
            HttpClient = factory.CreateClient();
            CleanDatabases();
        }

        private void CleanDatabases()
        {
            using (var scope = Factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var databaseContext = scopedServices.GetRequiredService<MyDocAppointmentDatabaseContext>();


                databaseContext.Hospitals.RemoveRange(databaseContext.Hospitals.ToList());
                databaseContext.Patients.RemoveRange(databaseContext.Patients.ToList());
                databaseContext.Appointments.RemoveRange(databaseContext.Appointments.ToList());
                databaseContext.Doctors.RemoveRange(databaseContext.Doctors.ToList());
                databaseContext.Medications.RemoveRange(databaseContext.Medications.ToList());
                databaseContext.Prescriptions.RemoveRange(databaseContext.Prescriptions.ToList());
                databaseContext.MedicationDosagePrescriptions.RemoveRange(databaseContext.MedicationDosagePrescriptions.ToList());
                //databaseContext.Schedules.RemoveRange(databaseContext.Schedules.ToList());
                databaseContext.SaveChanges();
            }
        }
    }
}
