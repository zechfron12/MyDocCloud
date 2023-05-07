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

        [HttpGet]
        public IActionResult GetAllBills()
        {
            var bills = billRepository.GetAll().Result;
            var billsDto = mapper.Map<IEnumerable<BillDto>>(bills);

            return Ok(billsDto);
        }

        [HttpGet("{billId:Guid}")]
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

        [HttpGet("{billId:Guid}/medications")]
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

        [HttpGet("{billId:Guid}/payment")]
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


        [HttpPost]
        public IActionResult Create([FromBody] CreateBillDto billDto)
        {
            var bill = mapper.Map<Bill>(billDto);

            billRepository.Add(bill);
            billRepository.SaveChanges();
            return Created(nameof(GetAllBills), bill);
            
        }

        [HttpPost("{billId:Guid}/medications")]
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

        [HttpPost("{billId:Guid}/payment")]
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

        [HttpDelete("{billId:Guid}")]
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

        [HttpDelete("{billId:Guid}/medications/{medicationId:Guid}")]
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
