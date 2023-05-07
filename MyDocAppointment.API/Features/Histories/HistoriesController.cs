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

        [HttpGet]
        public IActionResult GetAllHistories()
        {

            var histories = historyRepository.GetAll().Result;
            var historiesDto = mapper.Map<IEnumerable<HistoryDto>>(histories);

            return Ok(historiesDto);
        }

        [HttpGet("{historyId:Guid}/medications")]
        public IActionResult GetMedicationsFromHistory(Guid historyId)
        {
            var history = historyRepository.GetById(historyId).Result;
            if (history == null)
            {
                return NotFound("History with given id not found");
            }
            return Ok(history.MedicationDosageHistories);
        }
        
        [HttpPost]
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

        [HttpDelete("{historyId:Guid}")]
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
