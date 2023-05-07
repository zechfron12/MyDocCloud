using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyDocAppointment.API.Features.Appointments;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Doctors
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IRepository<Doctor> doctorRepository;
        private readonly IRepository<Appointment> appointmentRepository;
        private readonly IRepository<Patient> patientRepositroy;
        private readonly IMapper mapper;

        public DoctorsController(IRepository<Doctor> doctorRepository, IRepository<Appointment> appointmentRepository, IRepository<Patient> patientRepositroy, IMapper mapper)
        {
            this.doctorRepository = doctorRepository;
            this.appointmentRepository = appointmentRepository;
            this.patientRepositroy = patientRepositroy;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            var doctors = doctorRepository.GetAll().Result;
            var doctorsDto = mapper.Map<IEnumerable<DoctorDto>>(doctors);

            return Ok(doctorsDto);
        }

        [HttpGet("{doctorId:Guid}/appointments")]
        public IActionResult GetAppointmentsFromDoctor(Guid doctorId)
        {
            var appointments = appointmentRepository.Find(appointment => appointment.DoctorId == doctorId).Result;

            var appointmentDtos = mapper.Map<IEnumerable<AppointmentsDtoFromDoctor>>(appointments);
            return Ok(appointmentDtos);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateDoctorDto doctorDto)
        {
            if (doctorDto.FirstName != null && doctorDto.LastName != null && doctorDto.Specialization != null && doctorDto.Email != null && doctorDto.Phone != null && doctorDto.Title != null && doctorDto.Profession != null && doctorDto.Location != null)
            {
                var doctor = mapper.Map<Doctor>(doctorDto);

                doctorRepository.Add(doctor);
                doctorRepository.SaveChanges();
                return Created(nameof(GetAllDoctors), doctor);
            }
            return BadRequest("The fields in doctor must not be null");
        }

        [HttpPost("{doctorId:Guid}/reviews")]
        public IActionResult AddReviewToDoctor(Guid doctorId, [FromBody] CreateReviewDto reviewDto)
        {
            var doctor = doctorRepository.GetById(doctorId).Result;
            if (doctor == null)
            {
                return NotFound("Doctor with given id not found");
            }

            var result = doctor.AddReview(reviewDto.Review);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            doctorRepository.SaveChanges();
            return Ok(doctor);
        }


        [HttpPost("{doctorId:Guid}/appointments")]
        public IActionResult RegisterNewDoctorsToPatient(Guid doctorId, [FromBody] List<CreateAppointmentDto> appointmentDtos)
        {
            var doctor = doctorRepository.GetById(doctorId).Result;
            if (doctor == null)
            {
                return NotFound("Doctor with given id not found");
            }
            var appointments = new List<Appointment>();
            foreach (var a in appointmentDtos)
            {
                var appointment = mapper.Map<Appointment>(a);

                var patient = patientRepositroy.GetById(a.PatientId).Result;
                if(patient == null)
                {
                    return BadRequest($"Patient with given id ({a.PatientId}) not found");
                }
                var resultFromPatient = patient.AddAppointment(appointment);
                if(resultFromPatient.IsFailure) 
                {
                    return BadRequest(resultFromPatient.Error);
                }
                var resultFromDoctor = doctor.AddAppointment(appointment);
                if(resultFromDoctor.IsFailure)
                {
                    return BadRequest(resultFromDoctor.Error);
                }
                appointments.Add(appointment);
            }


            appointments.ForEach(a =>
            {
                appointmentRepository.Add(a);
            });
            appointmentRepository.SaveChanges();
            return Ok(appointments);
        }

        [HttpDelete("{doctorId:Guid}")]
        public IActionResult DeleteHospital(Guid doctorId)
        {
            try
            {
                doctorRepository.Delete(doctorId);
            }
            catch(ArgumentException e)
            {
                return NotFound(e.Message);
            }
            doctorRepository.SaveChanges();

            return NoContent();
        }


        [HttpPut("{doctorId:Guid}")]
        public IActionResult UpdateDoctor(Guid doctorId, [FromBody] Doctor doctor)
        {
            var doctorToChange = doctorRepository.GetById(doctorId).Result;

            if (doctorToChange == null)
            {
                return NotFound("Doctor with given id not found");
            }

            doctorToChange.UpdateDoctor(doctor);

            doctorRepository.SaveChanges();
            return Ok(doctorToChange);
        }

    }
}
