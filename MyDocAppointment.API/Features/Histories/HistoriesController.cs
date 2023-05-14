using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Histories
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class HistoriesController : ControllerBase
    {
        private readonly IRepository<History> historyRepository;
        private readonly IRepository<Patient> patientRepository;
        private readonly IMapper mapper;


        public HistoriesController(IRepository<History> historyRepository, IRepository<Patient> patientRepository, IMapper mapper)
        {
            this.historyRepository = historyRepository;
            this.patientRepository = patientRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get all histories.
        /// </summary>
        /// <response code="200">Returns the entire history collection</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllHistories()
        {

            var histories = historyRepository.GetAll().Result;
            var historiesDto = mapper.Map<IEnumerable<HistoryDto>>(histories);

            return Ok(historiesDto);
        }

        /// <summary>
        /// Get medications from a specific history payment.
        /// </summary>
        /// <response code="200">Returns the medication of a specific history</response>
        /// <response code="404">History with given id not found</response>
        [HttpGet("{historyId:Guid}/medications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetMedicationsFromHistory(Guid historyId)
        {
            var history = historyRepository.GetById(historyId).Result;
            if (history == null)
            {
                return NotFound("History with given id not found");
            }
            return Ok(history.MedicationDosageHistories);
        }

        /// <summary>
        /// Add a payment to history.
        /// </summary>
        /// <response code="201">Returns the new history</response>
        /// <response code="404">Patient with given id not found</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Create(Guid patientId, [FromBody] CreateHistoryDto historyDto)
        {
            var history = mapper.Map<History>(historyDto);

            var patient = patientRepository.GetById(patientId).Result;
            if(patient == null)
            {
                return BadRequest("Patient with given id not found");
            }
            history.AddPatientToHistory(patient);

            historyRepository.Add(history);
            historyRepository.SaveChanges();
            return Created(nameof(GetAllHistories), history);
        }

        /// <summary>
        /// Deletes a specific History recors.
        /// </summary>
        /// <response code="201">Success</response>
        /// <response code="404">History not found</response>
        [HttpDelete("{historyId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHistory(Guid historyId)
        {
            try
            {
                historyRepository.Delete(historyId);
            }
            catch(ArgumentException e)
            {
                return NotFound(e.Message);
            }
            historyRepository.SaveChanges();

            return NoContent();
        }
    }
}
