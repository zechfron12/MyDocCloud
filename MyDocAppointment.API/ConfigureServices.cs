using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyDocAppointment.BusinessLayer.Data;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;
using System.Reflection;

namespace MyDocAppointment.API
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Doctor>, DoctorRepository>();
            services.AddScoped<IRepository<Hospital>, HospitalRepository>();
            services.AddScoped<IRepository<Patient>, PatientRepository>();
            services.AddScoped<IRepository<History>, HistoryRepository>();
            services.AddScoped<IRepository<Medication>, MedicationRepositrory>();
            services.AddScoped<IRepository<Prescription>, PrescriptionRepository>();
            services.AddScoped<IRepository<Appointment>, AppointmentRepository>();
            services.AddScoped<IRepository<MedicationDosageHistory>, MedicationDosageHistoryRepository>();
            services.AddScoped<IRepository<MedicationDosagePrescription>, MedicationDosagePrescriptionRepository>();
            services.AddScoped<IRepository<Bill>, BillRepository>();
            services.AddScoped<IRepository<Payment>, PaymentRepository>();
            services.AddDbContext<MyDocAppointmentDatabaseContext>(options => options.UseSqlite("Data Source = dbMyDocAppointmentManagement.db"));
            return services;
        }
    }
}
