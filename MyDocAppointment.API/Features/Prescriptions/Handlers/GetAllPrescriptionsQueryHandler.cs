using AutoMapper;
using MediatR;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Prescriptions.Handlers
{
    public class GetAllPrescriptionsQueryHandler : IRequestHandler<GetAllPrescriptionsQuery, List<PrescriptionDto>>
    {
        private readonly IRepository<Prescription> repository;
        private readonly IMapper mapper;

        public GetAllPrescriptionsQueryHandler(IRepository<Prescription> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<List<PrescriptionDto>> Handle(GetAllPrescriptionsQuery request, CancellationToken cancellationToken)
        {
            var result = mapper.Map<List<PrescriptionDto>>(await repository.GetAll());
            return result;
        }
    }
}
