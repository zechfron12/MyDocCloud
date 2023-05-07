using FluentValidation;
using MyDocAppointment.API.Features.Histories;

namespace MyDocAppointment.API.Validations
{
    public class HistoryValidator:AbstractValidator<CreateHistoryDto>
    {
        public HistoryValidator() {
            RuleFor(e => e.StartDate).NotNull().NotEmpty();
            RuleFor(e => e.EndDate).NotNull().NotEmpty();
        }
    }
}
