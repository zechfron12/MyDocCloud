using MediatR;
using MyDocAppointment.API.Features.Appointments;

namespace MyDocAppointment.API.Features.Patients.Commands_and_Queries
{
    public class GetAllAppointmentsFromPatientQuery : IRequest<List<AppointmentsDtoFromPatient>>
    {
        public GetAllAppointmentsFromPatientQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
