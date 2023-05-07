using AutoMapper;
using MyDocAppointment.API.Features.Appointments;
using MyDocAppointment.API.Features.Bills;
using MyDocAppointment.API.Features.Doctors;
using MyDocAppointment.API.Features.Histories;
using MyDocAppointment.API.Features.Hospitals;
using MyDocAppointment.API.Features.MedicationDosage;
using MyDocAppointment.API.Features.Medications;
using MyDocAppointment.API.Features.Patients;
using MyDocAppointment.API.Features.Patients.Commands_and_Queries;
using MyDocAppointment.API.Features.Prescriptions;
using MyDocAppointment.BusinessLayer.Entities;

namespace MyDocAppointment.API
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<Appointment, CreateAppointmentDto>().ReverseMap();
            CreateMap<Appointment, AppointmentsDtoFromDoctor>().ReverseMap();
            CreateMap<Appointment, AppointmentsDtoFromPatient>().ReverseMap();


            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<Doctor, CreateDoctorDto>().ReverseMap();


            CreateMap<History, HistoryDto>().ReverseMap();
            CreateMap<History, CreateHistoryDto>().ReverseMap();

            CreateMap<Hospital, HospitalDto>().ReverseMap();
            CreateMap<Hospital, CreateHospitalDto>().ReverseMap();


            CreateMap<Medication, MedicationDto>().ReverseMap();
            CreateMap<Medication, CreateMedicationDto>().ReverseMap();


            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<Patient, CreatePatientCommand>().ReverseMap();

            CreateMap<Prescription, PrescriptionDto>().ReverseMap();
            CreateMap<Prescription, CreatePrescriptionCommnad>().ReverseMap();
            CreateMap<MedicationDosagePrescription, MedicationDosagePrescriptionDto>().ReverseMap();

            CreateMap<Bill, BillDto>().ReverseMap();
            CreateMap<Bill, CreateBillDto>().ReverseMap();
            
            CreateMap<Payment, PaymentDto>().ReverseMap();

        }
    }
}
