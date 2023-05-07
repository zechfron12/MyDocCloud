using FluentValidation;
using MyDocAppointment.API.Features.Medications;

namespace MyDocAppointment.API.Validations
{
    public class MedicationValidator : AbstractValidator<CreateMedicationDto>
    {
        public MedicationValidator() {
            RuleFor(m => m.Name).NotNull().NotEmpty();
            RuleFor(m => m.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock should not be negative.");
            RuleFor(m=>m.Capacity).GreaterThanOrEqualTo(0).WithMessage("Capacity should not be negative.");
            RuleFor(m=>m.Price).GreaterThanOrEqualTo(0).WithMessage("Price should not be negative.");  
        }
    }
}
