using FluentAssertions;
using MyDocAppointment.API.Features.Doctors;
using MyDocAppointment.API.Features.MedicationDosage;
using MyDocAppointment.API.Features.Medications;
using MyDocAppointment.API.Features.Patients;
using MyDocAppointment.API.Features.Prescriptions;
using System.Net.Http.Json;

namespace MyDocAppointment.Tests.ApiTests
{
    public class PrescriptionsControllerTests : BaseIntegrationTests<PrescriptionsController>
    {
        private const string ApiURL = "v1/api/Prescriptions";
        private const string ApiDoctorsURL = "v1/api/Doctors";
        private const string ApiPatientsURL = "v1/api/Patients";
        private const string ApiMedicationsURL = "v1/api/Medications";

        public PrescriptionsControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async void When_CreatedPrescription_Then_ShouldReturnPrescriptionInTheGetRequest()
        {
            // Arrange
            var createDoctorDto = CreateDoctorSUT();
            var createDoctorResponse = await HttpClient.PostAsJsonAsync(ApiDoctorsURL, createDoctorDto);
            var doctor = await createDoctorResponse.Content.ReadFromJsonAsync<DoctorDto>();

            doctor.Should().NotBeNull();

            var createPatientDto = CreatePatientSUT();
            var createPatientResponse = await HttpClient.PostAsJsonAsync(ApiPatientsURL, createPatientDto);
            var patient = await createPatientResponse.Content.ReadFromJsonAsync<PatientDto>();

            patient.Should().NotBeNull();

            var createMedicationDto = CreateMedicationDto();
            var createMedicationResponse = await HttpClient.PostAsJsonAsync(ApiMedicationsURL, createMedicationDto);
            var medication = await createMedicationResponse.Content.ReadFromJsonAsync<MedicationDto>();

            medication.Should().NotBeNull();

            if (doctor != null && patient != null && medication != null)
            {
                CreatePrescriptionDto prescriptionDto = CreateSUT(doctor.Id, patient.Id, medication.Id);

                // Act
                var createPrescriptionResponse = await HttpClient.PostAsJsonAsync(ApiURL, prescriptionDto);
                var getPrescriptionResult = await HttpClient.GetAsync(ApiURL);

                // Assert
                createPrescriptionResponse.EnsureSuccessStatusCode();
                createPrescriptionResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

                getPrescriptionResult.EnsureSuccessStatusCode();
                var prescriptions = await getPrescriptionResult.Content.ReadFromJsonAsync<List<PrescriptionDto>>();
                prescriptions.Should().HaveCount(1);
                prescriptions.Should().NotBeNull();
            }
        }


        [Fact]
        public async void When_DeletedPrescription_Then_ShouldReturnNoPrescriptionInTheGetRequest()
        {
            // Arrange
            var createDoctorDto = CreateDoctorSUT();
            var createDoctorResponse = await HttpClient.PostAsJsonAsync(ApiDoctorsURL, createDoctorDto);
            var doctor = await createDoctorResponse.Content.ReadFromJsonAsync<DoctorDto>();

            doctor.Should().NotBeNull();

            var createPatientDto = CreatePatientSUT();
            var createPatientResponse = await HttpClient.PostAsJsonAsync(ApiPatientsURL, createPatientDto);
            var patient = await createPatientResponse.Content.ReadFromJsonAsync<PatientDto>();

            patient.Should().NotBeNull();

            var createMedicationDto = CreateMedicationDto();
            var createMedicationResponse = await HttpClient.PostAsJsonAsync(ApiMedicationsURL, createMedicationDto);
            var medication = await createMedicationResponse.Content.ReadFromJsonAsync<MedicationDto>();

            medication.Should().NotBeNull();

            if (doctor != null && patient != null && medication != null)
            {

                CreatePrescriptionDto prescriptionDto = CreateSUT(doctor.Id, patient.Id, medication.Id);
                var createPrescriptionResponse = await HttpClient.PostAsJsonAsync(ApiURL, prescriptionDto);

                var prescription = await createPrescriptionResponse.Content.ReadFromJsonAsync<PrescriptionDto>();

                prescription.Should().NotBeNull();

                if (prescription != null)
                {
                    // Act
                    var resultResponse = await HttpClient.DeleteAsync
                        ($"{ApiURL}/{prescription.Id}");

                    // Assert
                    resultResponse.EnsureSuccessStatusCode();
                    resultResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
                }
            }
        }


        private static CreatePrescriptionDto CreateSUT(Guid doctorId, Guid patientId, Guid medicationId)
        {
            return new CreatePrescriptionDto
            {
                DoctorId = doctorId,
                PatientId = patientId,
                MedicationDosages = new List<MedicationDosagePrescriptionDto>()
                {
                    new MedicationDosagePrescriptionDto()
                    {
                        MedicationId = medicationId,
                        StartDate = DateTime.UtcNow.AddDays(1),
                        EndDate = DateTime.Now.AddDays(2),
                        Quantity = 1,
                        Frequency = 5
                    }
                }
            };
        }

        private static CreateDoctorDto CreateDoctorSUT()
        {
            return new CreateDoctorDto
            {
                FirstName = "Doctor",
                LastName = "Doctorescu",
                Specialization = "Diagnostic radiology",
                Email = "diagn@st.ic",
                Phone = "0712312312",
                Title = "doctor  docent",
                Profession = "--",
                Location = "Bosnia",
                Grade = 9,
                Reviews = 10
            };
        }

        private static CreatePatientDto CreatePatientSUT()
        {
            return new CreatePatientDto
            {
                FirstName = "Eu",
                LastName = "Tot eu",
                Email = "eu@datoteu.eu",
                Phone = "0712312312",
            };
        }

        private static CreateMedicationDto CreateMedicationDto()
        {
            return new CreateMedicationDto()
            {
                Name = "Aspirina",
                Stock = 10000,
                Unit = "capsule",
                Capacity = 1,
                Price = 1
            };
        }
    }
}
