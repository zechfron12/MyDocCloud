using AutoMapper;
using MediatR;
using MyDocAppointment.API.Features.Appointments;
using MyDocAppointment.API.Features.Patients.Commands_and_Queries;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Patients.Handlers
{
    public class GetAllAppointmentsFromPatientQueryHandler : IRequestHandler<GetAllAppointmentsFromPatientQuery, List<AppointmentsDtoFromPatient>>
    {
        private readonly IRepository<Appointment> appointmentRepository;
        private readonly IMapper mapper;

        public GetAllAppointmentsFromPatientQueryHandler(IRepository<Appointment> appointmentRepository,
            IMapper mapper)
        {
            this.appointmentRepository = appointmentRepository;
            this.mapper = mapper;
        }

        public async Task<List<AppointmentsDtoFromPatient>> Handle(GetAllAppointmentsFromPatientQuery request, CancellationToken cancellationToken)
        {
            var appointments = await appointmentRepository.Find(appointment => appointment.PatientId == request.Id);

            if (!appointments.Any())
            {
                throw new KeyNotFoundException("There is no patient with given id");
            }

            var appoinmentDtos = mapper.Map<List<AppointmentsDtoFromPatient>>(appointments);

            return appoinmentDtos;
        }
    }
}
