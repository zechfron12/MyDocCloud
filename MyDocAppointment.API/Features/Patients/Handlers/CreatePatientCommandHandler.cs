using AutoMapper;
using MediatR;
using MyDocAppointment.API.Features.Patients.Commands_and_Queries;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Patients.Handlers
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, PatientDto>
    {
        private readonly IRepository<Patient> patientRepository;
        private readonly IMapper mapper;
        
        public CreatePatientCommandHandler(IRepository<Patient> patientRepository, IMapper mapper)
        {
            this.patientRepository = patientRepository;
            this.mapper = mapper;
        }
        public async Task<PatientDto> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            if (request.FirstName != null && request.LastName != null && request.Email != null && request.Phone != null)
            {
                var patient = mapper.Map<Patient>(request);
                await patientRepository.Add(patient);
                patientRepository.SaveChanges();

                var patientDto = mapper.Map<PatientDto>(patient);
                return patientDto;
            }
            throw new BadHttpRequestException("The fields in patient must not be null");
        }
    }
}
