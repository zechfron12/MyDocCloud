using MediatR;

namespace MyDocAppointment.API.Features.Patients.Commands_and_Queries
{
    public class DeletePatientComand : IRequest
    {
        public DeletePatientComand(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; private set; }
    }
}
