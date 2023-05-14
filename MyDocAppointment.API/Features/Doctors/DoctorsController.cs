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

        /// <summary>
        /// Get all doctors.
        /// </summary>
        /// <response code="200">Returns all Doctors</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllDoctors()
        {
            var doctors = doctorRepository.GetAll().Result;
            var doctorsDto = mapper.Map<IEnumerable<DoctorDto>>(doctors);

            return Ok(doctorsDto);
        }

        /// <summary>
        /// Get all appontmnets of a doctor.
        /// </summary>
        /// <response code="200">Returns all appointmnets</response>
        [HttpGet("{doctorId:Guid}/appointments")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetAppointmentsFromDoctor(Guid doctorId)
        {
            var appointments = appointmentRepository.Find(appointment => appointment.DoctorId == doctorId).Result;

            var appointmentDtos = mapper.Map<IEnumerable<AppointmentsDtoFromDoctor>>(appointments);
            return Ok(appointmentDtos);
        }

        /// <summary>
        /// Add a doctor to the databse.
        /// </summary>
        /// <response code="201">Returns the newly created Doctor</response>
        /// <response code="400">The fields in doctor are null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Add reviews to a doctor.
        /// </summary>
        /// <response code="200">Return the updated doctor</response>
        /// <response code="400">Something went wrong</response>
        /// <response code="404">Doctor with given id not found</response>
        [HttpPost("{doctorId:Guid}/reviews")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Add an appointment to a doctor.
        /// </summary>
        /// /// <response code="200">Return the appointments</response>
        /// <response code="400">Something went wrong</response>
        /// <response code="404">Doctor with given id not found \n Patient with given not found</response>
        [HttpPost("{doctorId:Guid}/appointments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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


        /// <summary>
        /// Delete a Doctor.
        /// </summary>
        /// <response code="204">Success</response>
        /// <response code="404">Hospital not found</response>
        [HttpDelete("{doctorId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Update Doctor Data.
        /// </summary>
        /// <response code="200">Returns the newly change Doctor</response>
        /// <response code="404">Doctor with given id not found</response>
        [HttpPut("{doctorId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
