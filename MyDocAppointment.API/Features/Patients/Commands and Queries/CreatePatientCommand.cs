using MediatR;

namespace MyDocAppointment.API.Features.Patients.Commands_and_Queries
{
    public class CreatePatientCommand : IRequest<PatientDto>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
