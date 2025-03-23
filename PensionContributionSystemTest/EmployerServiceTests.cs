using Moq;
using PensionContributionSystem.Model;
using PensionContributionSystem.Repository.Interface;
using PensionContributionSystem.Service.Implementation;

namespace PensionContributionSystemTest
{
    public class EmployerServiceTests
    {
        private readonly Mock<IEmployerRepository> _mockEmployerRepository;
        private readonly EmployerService _employerService;

        public EmployerServiceTests()
        {
            _mockEmployerRepository = new Mock<IEmployerRepository>();
            _employerService = new EmployerService(_mockEmployerRepository.Object);
        }

        [Fact]
        public async Task RegisterEmployer_ValidEmployer_ShouldCallAddAsync()
        {
            // Arrange
            var employer = new Employer
            {
                CompanyName = "Test Company",
                IsActive = true
            };

            // Act
            await _employerService.RegisterEmployer(employer);

            // Assert
            _mockEmployerRepository.Verify(repo => repo.AddAsync(employer), Times.Once);
        }

        [Fact]
        public async Task RegisterEmployer_InvalidCompanyName_ShouldThrowException()
        {
            // Arrange
            var employer = new Employer
            {
                CompanyName = "", // Invalid company name
                IsActive = true
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _employerService.RegisterEmployer(employer));
        }

        [Fact]
        public async Task RegisterEmployer_InactiveEmployer_ShouldThrowException()
        {
            // Arrange
            var employer = new Employer
            {
                CompanyName = "Test Company",
                IsActive = false // Inactive employer
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _employerService.RegisterEmployer(employer));
        }
    }
}