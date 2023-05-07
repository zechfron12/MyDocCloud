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

        [HttpGet]
        public async Task<ActionResult<List<PatientDto>>> GetAllPatients()
        {
            var result = await mediator.Send(new GetAllPatientsQuery());
            return Ok(result);
        }

        [HttpGet("{patientId:Guid}/appointments")]
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

        [HttpPost]
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


        [HttpDelete("{patientId:Guid}")]
        public async Task<IActionResult> DeletePatient(Guid patientId)
        {
            await mediator.Send(new DeletePatientComand(patientId));
            return NoContent();
        }

    }
}
