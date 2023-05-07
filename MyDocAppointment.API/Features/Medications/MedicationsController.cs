using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Medications
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private readonly IRepository<Medication> medicationRepository;
        private readonly IMapper mapper;

        public MedicationsController(IRepository<Medication> medicationRepository, IMapper mapper)
        {
            this.medicationRepository = medicationRepository;
            this.mapper = mapper;
        }
      
        [HttpGet]
        public IActionResult GetAllMedications()
        {
            var medications = medicationRepository.GetAll().Result;
            var medicationsDto = mapper.Map<IEnumerable<MedicationDto>>(medications);
            return Ok(medicationsDto);
        }

        [HttpGet("{medicationId:Guid}")]
        public IActionResult GetMedicationById(Guid medicationId)
        {
            var medication = medicationRepository.GetById(medicationId).Result;

            if (medication == null)
            {
                return NotFound("There is no medication with given id");
            }

            var medicationDto = mapper.Map<MedicationDto>(medication);
            return Ok(medicationDto);
        }


        [HttpPost]
        public IActionResult Create([FromBody] CreateMedicationDto medicationDto)
        {
            if (medicationDto.Name != null && medicationDto.Unit != null)
            {
                var medication = mapper.Map<Medication>(medicationDto);
                medicationRepository.Add(medication);
                medicationRepository.SaveChanges();
                return Created(nameof(GetAllMedications), medication);
            }
            return BadRequest("The fields in medication must not be null");
        }


        [HttpDelete("{medicationId:Guid}")]
        public IActionResult DeleteMedication(Guid medicationId)
        {
            try
            {
                medicationRepository.Delete(medicationId);
            }
            catch(ArgumentException e)
            {
                return NotFound(e.Message);
            }
            medicationRepository.SaveChanges();

            return NoContent();
        }

        [HttpPut("{medicationId:Guid}")]
        public IActionResult UpdateMedication(Guid medicationId, [FromBody] Medication medication)
        {
            var medicationToChange = medicationRepository.GetById(medicationId).Result;

            if(medicationToChange==null)
            {
                return NotFound("There is no medication with the given id");
            }

            medicationToChange.UpdateMedication(medication);

            medicationRepository.SaveChanges();
            return Ok(medicationToChange);
        }
    }
}
