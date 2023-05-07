using AutoMapper;
using MediatR;
using MyDocAppointment.API.Features.Patients.Commands_and_Queries;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Patients.Handlers
{
    public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, List<PatientDto>>
    {
        private readonly IRepository<Patient> repository;
        private readonly IMapper mapper;

        public GetAllPatientsQueryHandler(IRepository<Patient> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<List<PatientDto>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var patients = await repository.GetAll();
            var patientDtos = mapper.Map<List<PatientDto>>(patients);

            return patientDtos;
        }
    }
}
