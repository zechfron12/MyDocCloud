using FluentValidation;
using MyDocAppointment.API.Features.Doctors;

namespace MyDocAppointment.API.Validations
{
    public class DoctorValidator:AbstractValidator<CreateDoctorDto>
    {
        public DoctorValidator() {
            RuleFor(d => d.FirstName).NotEmpty();
            RuleFor(d=> d.LastName).NotEmpty(); 
            RuleFor(d=>d.Specialization).NotEmpty();
            RuleFor(d => d.Email).EmailAddress();
            RuleFor(d => d.Phone).NotEmpty();
            RuleFor(d=>d.Title).NotEmpty();
            RuleFor(d=>d.Profession).NotEmpty();
            RuleFor(d=>d.Location).NotEmpty();
            RuleFor(d => d.Reviews).GreaterThanOrEqualTo(0).WithMessage("Reviews should not be negative.");
        }
    }
}
