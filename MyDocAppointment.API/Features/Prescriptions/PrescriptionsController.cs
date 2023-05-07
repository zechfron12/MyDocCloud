using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDocAppointment.API.Features.MedicationDosage;
using MyDocAppointment.API.Features.Prescriptions.Commands_and_Queries;

namespace MyDocAppointment.API.Features.Prescriptions
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IMediator mediator;


        public PrescriptionsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<PrescriptionDto>>> GetAllPrescriptions()
        {
            var result = await mediator.Send(new GetAllPrescriptionsQuery());
            return Ok(result);
        }

        [HttpGet("{prescriptionId:Guid}/medicationsDosages")]
        public async Task<ActionResult<List<MedicationDosagePrescriptionDto>>> GetAllMedicationsFromPrescription(Guid prescriptionId)
        {
            try
            {
                var medications = await mediator.Send(new GetAllMedicationsFromPresctriptionQuery(prescriptionId));
                return Ok(medications);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            
        }


        [HttpPost]
        public IActionResult CreatePrescription([FromBody] CreatePrescriptionCommnad command)
        {
            try
            {
                var result = mediator.Send(command);
                return Created(nameof(GetAllPrescriptions), result.Result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            

        }


        [HttpDelete("{prescriptionId:Guid}")]
        public async Task<IActionResult> DeletePrescription(Guid prescriptionId)
        {
            await mediator.Send(new DeletePrescriptionCommand(prescriptionId));
            return NoContent();
        }
    }
}
