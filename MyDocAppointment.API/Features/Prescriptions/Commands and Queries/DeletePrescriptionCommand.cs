using MediatR;

namespace MyDocAppointment.API.Features.Prescriptions.Commands_and_Queries
{
    public class DeletePrescriptionCommand : IRequest
    {
        public DeletePrescriptionCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
