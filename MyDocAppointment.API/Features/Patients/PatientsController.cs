using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDocAppointment.API.Features.Appointments;
using MyDocAppointment.API.Features.Patients.Commands_and_Queries;

namespace MyDocAppointment.API.Features.Patients
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IMediator mediator;

        public PatientsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all Patients.
        /// </summary>
        /// <response code="200">Returns all Patients</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<PatientDto>>> GetAllPatients()
        {
            var result = await mediator.Send(new GetAllPatientsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get a specific Patient.
        /// </summary>
        /// <response code="200">Returns patient</response>
        /// <response code="404">Patient not found</response>
        [HttpGet("{patientId:Guid}/appointments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AppointmentsDtoFromPatient>>> GetAllAppointmentsFromPatient(Guid patientId)
        {
            try
            {
                var result = await mediator.Send(new GetAllAppointmentsFromPatientQuery(patientId));
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return NotFound(ex.Message);
            }
            

        }

        /// <summary>
        /// Create a Patient.
        /// </summary>
        /// <response code="201">Returns patient</response>
        /// <response code="400">Null fields</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public async Task<ActionResult<PatientDto>> CreatePatient([FromBody] CreatePatientCommand command)
        {
            try
            {
                var result = await mediator.Send(command);
                return Created(nameof(GetAllPatients),result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete a specific Patient.
        /// </summary>
        /// <response code="203">Success</response>
        /// <response code="404">Patient not found </response>
        [HttpDelete("{patientId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePatient(Guid patientId)
        {
            await mediator.Send(new DeletePatientComand(patientId));
            return NoContent();
        }

    }
}
