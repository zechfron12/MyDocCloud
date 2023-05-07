using AutoMapper;
using MediatR;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Prescriptions.Handlers
{
    public class CreateEmployeeCommandHandler :
         IRequestHandler<CreatePrescriptionCommnad,
             PrescriptionDto>
    {
        private readonly IRepository<Prescription> prescriptionRepository;
        private readonly IRepository<Patient> patientRepository;
        private readonly IRepository<Doctor> doctorRepository;
        private readonly IRepository<Medication> medicationRepository;
        private readonly IMapper mapper;

        public CreateEmployeeCommandHandler(IRepository<Prescription> prescriptionRepository,
            IRepository<Patient> patientRepository,
            IRepository<Doctor> doctorRepository,
            IRepository<Medication> medicationRepository,
            IMapper mapper)
        {
            this.prescriptionRepository = prescriptionRepository;
            this.patientRepository = patientRepository;
            this.doctorRepository = doctorRepository;
            this.medicationRepository = medicationRepository;
            this.mapper = mapper;
        }
        public async Task<PrescriptionDto>
            Handle(CreatePrescriptionCommnad request,
            CancellationToken cancellationToken)
        {
            //

            var prescription = new Prescription();

            var doctor = doctorRepository.GetById(request.DoctorId).Result;
            var patient = patientRepository.GetById(request.PatientId).Result;

            if (doctor == null)
            {
                throw new BadHttpRequestException("There is no doctor with given id");
            }
            if (patient == null)
            {
                throw new BadHttpRequestException("There is no patient with given id");
            }
            doctor.AddPrescription(prescription);
            patient.AddPrescription(prescription);

            var medicationDosages = new List<MedicationDosagePrescription>();
            if (request.MedicationDosages != null)
            {
                foreach (var medicationDosageDto in request.MedicationDosages)
                {
                    var medication = medicationRepository.GetById(medicationDosageDto.MedicationId).Result;

                    if (medication == null)
                    {
                        throw new BadHttpRequestException("Medication with given id not found");
                    }

                    var medicationDosage = new MedicationDosagePrescription(medicationDosageDto.StartDate, medicationDosageDto.EndDate, medicationDosageDto.Quantity, medicationDosageDto.Frequency);
                    medicationDosage.AddMedication(medication);
                    medicationDosages.Add(medicationDosage);

                }

                prescription.AddMedications(medicationDosages);


                await prescriptionRepository.Add(prescription);
                prescriptionRepository.SaveChanges();
                return mapper.Map<PrescriptionDto>(prescription);
            }
            throw new BadHttpRequestException("There are no medications for this prescription");
        }
    }
}
