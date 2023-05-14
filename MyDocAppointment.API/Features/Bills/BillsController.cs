using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyDocAppointment.API.Features.Medications;
using MyDocAppointment.BusinessLayer.Entities;
using MyDocAppointment.BusinessLayer.Repositories;

namespace MyDocAppointment.API.Features.Bills
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly IRepository<Bill> billRepository;
        private readonly IRepository<Medication> medicationRepository;
        private readonly IRepository<Payment> paymentRepository;
        private readonly IMapper mapper;

        public BillsController(IRepository<Bill> billRepository, IRepository<Medication> medicationRepository, IRepository<Payment> paymentRepository, IMapper mapper)
        {
            this.billRepository = billRepository;
            this.medicationRepository = medicationRepository;
            this.paymentRepository = paymentRepository;
            this.mapper = mapper;
        }
        /// <summary>
        /// Get all Bills.
        /// </summary>
        [HttpGet]
        public IActionResult GetAllBills()
        {
            var bills = billRepository.GetAll().Result;
            var billsDto = mapper.Map<IEnumerable<BillDto>>(bills);

            return Ok(billsDto);
        }
        /// <summary>
        /// Get a specific bill.
        /// </summary>
        /// <response code="201">Success</response>
        /// <response code="400">There is no bill with given id</response>
        [HttpGet("{billId:Guid}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBillById(Guid billId)
        {
            var bill = billRepository.GetById(billId).Result;

            if (bill == null)
            {
                return NotFound("There is no bill with given id");
            }

            var billDto = mapper.Map<BillDto>(bill);
            return Ok(billDto);
        }
        /// <summary>
        /// Get medications of a specific bill.
        /// </summary>
        /// <response code="201">Ok</response>
        /// <response code="404">Bill with given id not found</response>
        [HttpGet("{billId:Guid}/medications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMedicationsFromBill(Guid billId)
        {
            var bill = billRepository.GetById(billId).Result;
            if (bill == null)
            {
                return NotFound("Bill with given id not found");
            }

            var medications = medicationRepository.Find(medication => medication.Bills.Contains(bill)).Result;

            var medicationDtos = mapper.Map<IEnumerable<MedicationDto>>(medications);

            return Ok(medicationDtos);
        }
        /// <summary>
        /// Get payment details of a specific bill.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="404">Bill with given id not found"</response>
        [HttpGet("{billId:Guid}/payment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetPaymentFromBill(Guid billId)
        {
            var bill = billRepository.GetById(billId).Result;
            if (bill == null)
            {
                return NotFound("Bill with given id not found");
            }
            var payment = paymentRepository.Find(payment => payment.BillId == billId).Result;


            return Ok(payment);
        }

        /// <summary>
        /// Create a bill.
        /// </summary>
        /// <response code="201">Created</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Create([FromBody] CreateBillDto billDto)
        {
            var bill = mapper.Map<Bill>(billDto);

            billRepository.Add(bill);
            billRepository.SaveChanges();
            return Created(nameof(GetAllBills), bill);
            
        }

        /// <summary>
        /// Add medications to a Bill.
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">If Bill with given id not found or Medication with given id not found</response>
        [HttpPost("{billId:Guid}/medications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RegisterMedicationsToBill(Guid billId, [FromBody] List<MedicationDto> medicationDtos)
        {
            var bill = billRepository.GetById(billId).Result;
            if (bill == null)
            {
                return NotFound("Bill with given id not found");
            }

            List<Medication> medications = new List<Medication>();

            foreach(var m in medicationDtos)
            {
                var medication = medicationRepository.GetById(m.Id).Result;

                if (medication == null)
                {
                    return BadRequest("Medication with given id not found");
                }

                medications.Add(medication);
            }

            bill.AddMedications(medications);
            billRepository.Update(bill);
            billRepository.SaveChanges();
            return Ok(medications);
        }

        /// <summary>
        /// Add payment to a bill.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">The bill already has a payment</response>
        /// <response code="400">Bill with given id not found</response>
        [HttpPost("{billId:Guid}/payment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RegisterPaymentToBill(Guid billId, [FromBody] PaymentDto paymentDto)
        {
            var bill = billRepository.GetById(billId).Result;
            if (bill == null)
            {
                return NotFound("Bill with given id not found");
            }

            if (bill.Payment != null)
            {
                return BadRequest("The bill already has a payment.");
            }

            foreach(var m in bill.Medications)
            {
                if(m.Stock < 1)
                {
                    return BadRequest($"Medication {m.Name} does not have enough stock.");
                }
            }

            var payment = mapper.Map<Payment>(paymentDto);
            payment.AddBillToPayment(bill);
            paymentRepository.Add(payment);

            foreach (var m in bill.Medications)
            {
                var result = m.UpdateStock();
                if (result.IsFailure)
                {
                    return BadRequest(result.Error);
                }
            }

            bill.AddPaymentToBill(payment);
            billRepository.Update(bill);

            paymentRepository.SaveChanges();
            billRepository.SaveChanges();

            return Ok(payment);
        }

        /// <summary>
        /// Delete a bill.
        /// </summary>
        /// <response code="204">Success</response>
        /// <response code="404">Bill not found</response>
        [HttpDelete("{billId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteBill(Guid billId)
        {
            try
            {
                billRepository.Delete(billId);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            billRepository.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Delete a medication from a bill.
        /// </summary>
        /// <response code="204">Success</response>
        /// <response code="404">If no entity with given Id was found</response>
        [HttpDelete("{billId:Guid}/medications/{medicationId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteMedicationFromBill(Guid billId, Guid medicationId)
        {
            var billToChange = billRepository.GetById(billId).Result;

            if (billToChange == null)
            {
                return NotFound("Bill with given id not found");
            }

            var medicationToRemove = medicationRepository.GetById(medicationId).Result;

            if (medicationToRemove == null)
            {
                return NotFound("Medication with given id not found");
            }

            billToChange.Medications.Remove(medicationToRemove);

            billRepository.Update(billToChange); 
            billRepository.SaveChanges();

            billToChange.RemoveMedication(medicationToRemove);

            return NoContent();
        }

        
    }
}
