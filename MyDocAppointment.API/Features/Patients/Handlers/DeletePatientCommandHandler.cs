using MediatR;
using MyDocAppointment.API.Features.Patients.Commands_and_Queries;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Patients.Handlers
{
    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientComand>
    {
        private readonly IRepository<Patient> patientRepository;

        public DeletePatientCommandHandler(IRepository<Patient> patientRepository)
        {
            this.patientRepository = patientRepository;
        }
        public Task<Unit> Handle(DeletePatientComand request, CancellationToken cancellationToken)
        {
            patientRepository.Delete(request.Id);
            patientRepository.SaveChanges();

            return Task.FromResult(Unit.Value);
        }
    }
}
