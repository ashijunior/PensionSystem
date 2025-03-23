using Moq;
using PensionContributionSystem.BackgroundJobs;
using PensionContributionSystem.Model;
using PensionContributionSystem.Repository.Interface;
using PensionContributionSystem.Service.Interface;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PensionContributionSystemTest
{
    public class HangfireJobsTests
    {
        private readonly Mock<IContributionService> _mockContributionService;
        private readonly Mock<IMemberRepository> _mockMemberRepository;
        private readonly Mock<IMemberService> _mockMemberService;
        private readonly Mock<ILogger<HangfireJobs>> _mockLogger;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly HangfireJobs _hangfireJobs;

        public HangfireJobsTests()
        {
            _mockContributionService = new Mock<IContributionService>();
            _mockMemberRepository = new Mock<IMemberRepository>();
            _mockMemberService = new Mock<IMemberService>();
            _mockLogger = new Mock<ILogger<HangfireJobs>>();
            _mockNotificationService = new Mock<INotificationService>();

            // Initialize HangfireJobs with all required dependencies
            _hangfireJobs = new HangfireJobs(
                _mockContributionService.Object,
                _mockMemberRepository.Object,
                _mockMemberService.Object,
                _mockLogger.Object,
                _mockNotificationService.Object // Add the notification service
            );
        }

        [Fact]
        public async Task ValidateContributions_ValidContributions_ShouldUpdateValidationStatus()
        {
            // Arrange
            var contributions = new List<Contribution>
            {
                new Contribution { ContributionID = 1, Amount = 100, ContributionDate = DateTime.UtcNow, IsValidated = false },
                new Contribution { ContributionID = 2, Amount = 200, ContributionDate = DateTime.UtcNow, IsValidated = false }
            };

            _mockContributionService
                .Setup(service => service.GetUnvalidatedContributionsAsync())
                .ReturnsAsync(contributions);

            // Act
            await _hangfireJobs.ValidateContributions();

            // Assert
            _mockContributionService.Verify(service => service.UpdateContributionAsync(It.IsAny<Contribution>()), Times.Exactly(2));
        }

        [Fact]
        public async Task HandleFailedTransactions_ShouldRetryAndNotify()
        {
            // Arrange
            var failedTransactions = new List<Contribution>
            {
                new Contribution { ContributionID = 1, MemberID = 1, Amount = 100 }
            };

            _mockContributionService
                .Setup(service => service.GetFailedTransactionsAsync())
                .ReturnsAsync(failedTransactions);

            _mockContributionService
                .Setup(service => service.AddContributionAsync(It.IsAny<Contribution>()))
                .ThrowsAsync(new Exception("Failed"));

            // Act
            await _hangfireJobs.HandleFailedTransactions();

            // Assert
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error, // Verify the log level
                    It.IsAny<EventId>(), // Verify the event ID (if applicable)
                    It.Is<It.IsAnyType>((v, t) => true), // Verify the log message
                    It.IsAny<Exception>(), // Verify the exception
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()), // Verify the formatter (nullable Exception)
                Times.Once); // Verify that the log was called once

            // Verify that the notification service was called
            _mockNotificationService.Verify(
                service => service.SendNotificationAsync(It.IsAny<int>(), It.IsAny<string>()),
                Times.Once);
        }
    }
}