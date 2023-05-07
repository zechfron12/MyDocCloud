using FluentAssertions;
using MyDocAppointment.API.Features.Appointments;
using MyDocAppointment.API.Features.Doctors;
using MyDocAppointment.API.Features.Patients;
using System.Net.Http.Json;

namespace MyDocAppointment.Tests.ApiTests
{
    public class DoctorsControllerTests : BaseIntegrationTests<DoctorsController>
    {
        private const string DoctorsApiURL = "v1/api/Doctors";
        private const string PatientsApiUrl = "v1/api/Patients";

        public DoctorsControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async void When_CreatedDoctor_Then_ShouldReturnDoctorInTheGetRequest()
        {
            // Arrange
            CreateDoctorDto createDoctorDto = CreateSUT();
            // Act
            var createDoctorResponse = await HttpClient.PostAsJsonAsync(DoctorsApiURL, createDoctorDto);
            var getDoctorResult = await HttpClient.GetAsync(DoctorsApiURL);
            // Assert
            createDoctorResponse.EnsureSuccessStatusCode();
            createDoctorResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            getDoctorResult.EnsureSuccessStatusCode();
            var doctors = await getDoctorResult.Content.ReadFromJsonAsync<List<DoctorDto>>();
            doctors.Should().HaveCount(1);
            doctors.Should().NotBeNull();
        }

        [Fact]
        public async void When_RegisterAppointmentsToDoctor_Then_ShouldReturnAppointmentsInTheGetRequest()
        {
            // Arrange
            CreateDoctorDto createDoctorDto = CreateSUT();
            var createDoctorResponse = await HttpClient.PostAsJsonAsync(DoctorsApiURL, createDoctorDto);
            var doctor = await createDoctorResponse.Content.ReadFromJsonAsync<DoctorDto>();

            doctor.Should().NotBeNull();

            CreatePatientDto createPatientDto = CreatePatientSUT();
            var createPatientResponse = await HttpClient.PostAsJsonAsync(PatientsApiUrl, createPatientDto);
            var patient = await createPatientResponse.Content.ReadFromJsonAsync<PatientDto>();

            patient.Should().NotBeNull();

            if (patient != null)
            {
                var appointments = new List<AppointmentsDtoFromDoctor>
                {
                    new AppointmentsDtoFromDoctor
                    {
                        StartTime = DateTime.Now.AddDays(1),
                        EndTime = DateTime.Now.AddDays(1).AddHours(1),
                        PatientId = patient.Id
                    },
                    new AppointmentsDtoFromDoctor
                    {
                        StartTime = DateTime.Now.AddDays(2),
                        EndTime = DateTime.Now.AddDays(2).AddHours(1),
                        PatientId = patient.Id
                    }
                };
                if (doctor != null)
                {
                    // Act
                    var resultResponse = await HttpClient.PostAsJsonAsync
                    ($"{DoctorsApiURL}/{doctor.Id}/appointments", appointments);

                    // Assert
                    resultResponse.EnsureSuccessStatusCode();
                    resultResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
                }
            }
        }
        
        [Fact]
        public async void When_DeletedDoctor_Then_ShouldReturnNoDoctorInTheGetRequest()
        {
            // Arrange
            CreateDoctorDto createDoctorDto = CreateSUT();
            var createDoctorResponse = await HttpClient.PostAsJsonAsync(DoctorsApiURL, createDoctorDto);
            var doctor = await createDoctorResponse.Content.ReadFromJsonAsync<DoctorDto>();

            doctor.Should().NotBeNull();

            if (doctor != null)
            {
                // Act
                var resultResponse = await HttpClient.DeleteAsync
                    ($"{DoctorsApiURL}/{doctor.Id}");

                // Assert
                resultResponse.EnsureSuccessStatusCode();
                resultResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            }
        }
        private static CreateDoctorDto CreateSUT()
        {
            return new CreateDoctorDto
            {
                FirstName = "FirstName1",
                LastName = "LastName1",
                Specialization = "Dermatology",
                Email = "doctor@gmail.com",
                Phone = "1234567890",
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
    }
}
