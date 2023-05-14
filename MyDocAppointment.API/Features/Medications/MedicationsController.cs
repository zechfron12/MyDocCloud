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

        /// <summary>
        /// Get all medications.
        /// </summary>
        /// <response code="200">Returns a list of medications</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllMedications()
        {
            var medications = medicationRepository.GetAll().Result;
            var medicationsDto = mapper.Map<IEnumerable<MedicationDto>>(medications);
            return Ok(medicationsDto);
        }

        /// <summary>
        /// Get a specific medication.
        /// </summary>
        /// <response code="200">Returns a medication</response>
        /// <response code="404">Medication not found</response>
        [HttpGet("{medicationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Add a medication.
        /// </summary>
        /// <response code="201">Returns the newly created medication</response>
        /// <response code="400">he fields in medication must not be null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Deletes a specific Medication.
        /// </summary>
        /// <response code="204">Success</response>
        /// <response code="404">Medication not found</response>
        [HttpDelete("{medicationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Update a specific medication.
        /// </summary>
        /// <response code="200">Returns the newly changed medication</response>
        /// <response code="404">There is no medication with the given id</response>
        [HttpPut("{medicationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
