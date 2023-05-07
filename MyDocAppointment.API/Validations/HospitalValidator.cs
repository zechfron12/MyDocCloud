using FluentValidation;
using MyDocAppointment.API.Features.Hospitals;

namespace MyDocAppointment.API.Validations
{
    public class HospitalValidator: AbstractValidator<CreateHospitalDto>
    {
        public HospitalValidator() {
            RuleFor(h=>h.Name).NotNull().NotEmpty();
            RuleFor(h=>h.Address).NotNull().NotEmpty();
            RuleFor(h=>h.Phone).NotNull().MaximumLength(12);
        }  
    }
}
