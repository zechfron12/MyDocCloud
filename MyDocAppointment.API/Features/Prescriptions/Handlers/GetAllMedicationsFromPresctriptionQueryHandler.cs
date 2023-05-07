using AutoMapper;
using MediatR;
using MyDocAppointment.API.Features.MedicationDosage;
using MyDocAppointment.API.Features.Prescriptions.Commands_and_Queries;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Prescriptions.Handlers
{
    public class GetAllMedicationsFromPresctriptionQueryHandler : IRequestHandler<GetAllMedicationsFromPresctriptionQuery, List<MedicationDosagePrescriptionDto>>
    {
        private readonly IRepository<Prescription> prescriptionRepository;
        private readonly IMapper mapper;

        public GetAllMedicationsFromPresctriptionQueryHandler(IRepository<Prescription> presprescriptionRepositorycript,
            IMapper mapper)
        {
            this.prescriptionRepository = presprescriptionRepositorycript;
            this.mapper = mapper;
        }

        public async Task<List<MedicationDosagePrescriptionDto>> Handle(GetAllMedicationsFromPresctriptionQuery request, CancellationToken cancellationToken)
        {
            var prescription = await prescriptionRepository.GetById(request.Id);
            if (prescription == null)
            {
                throw new KeyNotFoundException("Prescription with given id not found");
            }

            var medications = prescription.MedicationDosagePrescriptions;

            var medicationDtos = mapper.Map<List<MedicationDosagePrescriptionDto>>(medications);

            return medicationDtos;
        }
    }
}
