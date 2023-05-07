using MediatR;
using MyDocAppointment.API.Features.Prescriptions.Commands_and_Queries;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Prescriptions.Handlers
{
    public class DeletePrescritionCommandHandler : IRequestHandler<DeletePrescriptionCommand>
    {
        private readonly IRepository<Prescription> repositoy;

        public DeletePrescritionCommandHandler(IRepository<Prescription> repository)
        {
            this.repositoy = repository;
        }

        public Task<Unit> Handle(DeletePrescriptionCommand request, CancellationToken cancellationToken)
        {
            repositoy.Delete(request.Id);
            repositoy.SaveChanges();
            return Task.FromResult(Unit.Value);
        }
    }
}
