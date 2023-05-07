using MediatR;

namespace MyDocAppointment.API.Features.Prescriptions
{
    public class GetAllPrescriptionsQuery : IRequest<List<PrescriptionDto>>
    {
    }
}
