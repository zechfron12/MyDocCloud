using FluentValidation;
using MyDocAppointment.API.Features.Appointments;

namespace MyDocAppointment.API.Validations
{
    public class AppointmentValidator:AbstractValidator<CreateAppointmentDto>
    {
        public AppointmentValidator() {
            RuleFor(a => a.StartTime).NotNull().NotEmpty();
            RuleFor(a => a.StartTime).Must(s => s >= DateTime.Now).WithMessage("Appointment should be in the future.");
            RuleFor(a => a.EndTime).NotNull().NotEmpty();
            RuleFor(a => a.DoctorId).NotNull().NotEmpty();
            RuleFor(a => a.PatientId).NotNull().NotEmpty();
        }
    }
}
