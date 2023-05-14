using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyDocAppointment.API.Features.Doctors;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Hospitals
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly IRepository<Hospital> hospitalRepository;
        private readonly IRepository<Doctor> doctorRepository;
        private readonly IMapper mapper;

        public HospitalsController(IRepository<Hospital> hospitalRepository, IRepository<Doctor> doctorRepository,IMapper mapper)
        {
            this.hospitalRepository = hospitalRepository;
            this.doctorRepository = doctorRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all hospitals.
        /// </summary>
        /// <response code="200">Returns all hospitals</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHospitals()
        {
            var hospitals = hospitalRepository.GetAll().Result;
            var hospitalsDto= mapper.Map<IEnumerable<HospitalDto>>(hospitals);

            return Ok(hospitalsDto);
        }

        /// <summary>
        /// Get all Doctors from a specific Hospidat.
        /// </summary>
        /// <response code="200">Returns all doctors from a hospital</response>
        [HttpGet("{hospitalId:Guid}/doctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetAllDoctorsFromHostpital(Guid hospitalId)
        {
            var doctors = doctorRepository.Find(doctor => doctor.HospitalId == hospitalId).Result;
            var doctorDtos = mapper.Map<IEnumerable<DoctorDto>>(doctors);

            return Ok(doctorDtos);
        }

        /// <summary>
        /// Create a Hospital.
        /// </summary>
        /// <response code="201">Returns the created hospital</response>
        /// <response code="400">The fields in hospital must not be null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult CreateHospital([FromBody] CreateHospitalDto hospitalDto)
        {
            if (hospitalDto.Name != null && hospitalDto.Address != null && hospitalDto.Phone != null)
            {
                var hospital = mapper.Map<Hospital>(hospitalDto);
                hospitalRepository.Add(hospital);
                hospitalRepository.SaveChanges();
                return Created(nameof(GetAllHospitals), hospital);
            }
            return BadRequest("The fields in hospital must not be null");
        }

        /// <summary>
        /// Add doctors to a Hospital.
        /// </summary>
        /// <response code="204">Success</response>
        /// <response code="404">Hospital with given id not found</response>
        [HttpPost("{hospitalId:Guid}/doctors")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RegisterNewDoctorsToHospital(Guid hospitalId, [FromBody] List<CreateDoctorDto> doctorsDtos)
        {

            var hospital = hospitalRepository.GetById(hospitalId).Result;
            if (hospital == null)
            {
                return NotFound("Hospital with given id not found");
            }

            var doctors = mapper.Map<List<Doctor>>(doctorsDtos);

            var result = hospital.AddDoctors(doctors);


            doctors.ForEach(d =>
            {
                doctorRepository.Add(d);
            });
            doctorRepository.SaveChanges();

            return result.IsSuccess ? NoContent() : BadRequest();
        }

        /// <summary>
        /// Delete a specific Hospital.
        /// </summary>
        /// <response code="204">Success</response>
        /// <response code="404">Hospital with given id not found</response>

        [HttpDelete("{hospitalId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHospital(Guid hospitalId)
        {
            hospitalRepository.Delete(hospitalId);
            hospitalRepository.SaveChanges();

            return NoContent();
        }

        
        
    }
}
