using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Appointments
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IRepository<Appointment> appointmentRepository;
        private readonly IRepository<Patient> patientRepository;
        private readonly IRepository<Doctor> doctorRepository;
        private readonly IMapper mapper;

        public AppointmentsController(IRepository<Appointment> appointmentRepository, IRepository<Patient> patientRepository, IRepository<Doctor> doctorRepository, IMapper mapper)
        {
            this.appointmentRepository = appointmentRepository;
            this.patientRepository = patientRepository;
            this.doctorRepository = doctorRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public IActionResult GetAllAppointments()
        {

            var appointments = appointmentRepository.GetAll().Result;
            var appointmentsDto = mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            
            return Ok(appointmentsDto);
            
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateAppointmentDto appointmentDto)
        {
            var appointment = mapper.Map<Appointment>(appointmentDto);
            var doctor = doctorRepository.GetById(appointmentDto.DoctorId).Result;
            var patient = patientRepository.GetById(appointmentDto.PatientId).Result;
            if(doctor == null)
            {
                return BadRequest("Doctor with given id not found");
            }
            if(patient == null)
            {
                return BadRequest("Patient with given id not found");
            }

            var resultFromDoctor = doctor.AddAppointment(appointment);
            if(resultFromDoctor.IsFailure)
            {
                return BadRequest(resultFromDoctor.Error);
            }
            var resultFromPatient =  patient.AddAppointment(appointment);
            if(resultFromPatient.IsFailure)
            {
                return BadRequest(resultFromPatient.Error);
            }

            appointment.AddPatientToAppointment(patient);
            appointment.AddDoctorToAppointment(doctor);

            appointmentRepository.Add(appointment);
            appointmentRepository.SaveChanges();

            return Created(nameof(GetAllAppointments), appointment);
        }

        [HttpDelete("{appointmentId:Guid}")]
        public IActionResult DeleteAppointment(Guid appointmentId)
        {
            try
            {
                appointmentRepository.Delete(appointmentId);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            appointmentRepository.SaveChanges();

            return NoContent();
        }
    }
}
