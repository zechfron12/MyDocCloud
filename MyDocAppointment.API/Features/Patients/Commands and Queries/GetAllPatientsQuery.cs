using MediatR;

namespace MyDocAppointment.API.Features.Patients.Commands_and_Queries
{
    public class GetAllPatientsQuery : IRequest<List<PatientDto>>
    {
    }
}
