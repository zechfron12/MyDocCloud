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


        /// <summary>
        /// Get all Prescriptions.
        /// </summary>
        /// <response code="200">Returns all Prescriptions</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<List<PrescriptionDto>>> GetAllPrescriptions()
        {
            var result = await mediator.Send(new GetAllPrescriptionsQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get medications from a specific Prescription.
        /// </summary>
        /// <response code="200">Returns a list of medications of a specific Prescription</response>
        /// <response code="404">Prescription not found</response>
        [HttpGet("{prescriptionId:Guid}/medicationsDosages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Create a prescription.
        /// </summary>
        /// <response code="201">Returns the created Prescription</response>
        /// <response code="400">Prescription not found</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Delete a specific Prescription.
        /// </summary>
        /// <response code="203">Success</response>
        /// <response code="400">Prescription not found</response>
        [HttpDelete("{prescriptionId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePrescription(Guid prescriptionId)
        {
            await mediator.Send(new DeletePrescriptionCommand(prescriptionId));
            return NoContent();
        }
    }
}
