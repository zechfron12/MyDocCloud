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

        [HttpGet]
        public IActionResult GetAllHospitals()
        {
            var hospitals = hospitalRepository.GetAll().Result;
            var hospitalsDto= mapper.Map<IEnumerable<HospitalDto>>(hospitals);

            return Ok(hospitalsDto);
        }

        [HttpGet("{hospitalId:Guid}/doctors")]
        public IActionResult GetAllDoctorsFromHostpital(Guid hospitalId)
        {
            var doctors = doctorRepository.Find(doctor => doctor.HospitalId == hospitalId).Result;
            var doctorDtos = mapper.Map<IEnumerable<DoctorDto>>(doctors);

            return Ok(doctorDtos);
        }

        [HttpPost]
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

        [HttpPost("{hospitalId:Guid}/doctors")]
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

        [HttpDelete("{hospitalId:Guid}")]
        public IActionResult DeleteHospital(Guid hospitalId)
        {
            hospitalRepository.Delete(hospitalId);
            hospitalRepository.SaveChanges();

            return NoContent();
        }

        
        
    }
}
