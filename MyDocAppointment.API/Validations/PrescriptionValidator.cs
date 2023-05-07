using FluentValidation;
using MyDocAppointment.API.Features.Prescriptions;

namespace MyDocAppointment.API.Validations
{
    public class PrescriptionValidator: AbstractValidator<CreatePrescriptionDto>
    {
        public PrescriptionValidator()
        {
            RuleFor(p => p.DoctorId).NotNull().NotEmpty();
            RuleFor(p=>p.PatientId).NotNull().NotEmpty();
            RuleForEach(u => u.MedicationDosages)
                .ChildRules(m =>
                {
                    m.RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity of medication dosages should not be negative.");
                    m.RuleFor(x => x.Frequency).GreaterThanOrEqualTo(0).WithMessage("Frequency of medication dosages should not be negative.");
                });
        }
    }
}
