using Moq;
using PensionContributionSystem.Model;
using PensionContributionSystem.Repository.Interface;
using PensionContributionSystem.Service.Implementation;


namespace PensionContributionSystemTest
{
    public class ContributionServiceTests
    {
        private readonly Mock<IContributionRepository> _mockContributionRepository;
        private readonly Mock<IMemberRepository> _mockMemberRepository;
        private readonly ContributionService _contributionService;

        public ContributionServiceTests()
        {
            _mockContributionRepository = new Mock<IContributionRepository>();
            _mockMemberRepository = new Mock<IMemberRepository>();
            _contributionService = new ContributionService(_mockContributionRepository.Object, _mockMemberRepository.Object);
        }

        [Fact]
        public async Task AddContributionAsync_ValidContribution_ShouldCallAddAsync()
        {
            // Arrange
            var contribution = new Contribution
            {
                MemberID = 1,
                ContributionType = ContributionType.Monthly,
                Amount = 100,
                ContributionDate = DateTime.UtcNow
            };

            // Act
            await _contributionService.AddContributionAsync(contribution);

            // Assert
            _mockContributionRepository.Verify(repo => repo.AddAsync(contribution), Times.Once);
        }

        [Fact]
        public async Task AddContributionAsync_InvalidAmount_ShouldThrowException()
        {
            // Arrange
            var contribution = new Contribution
            {
                MemberID = 1,
                ContributionType = ContributionType.Monthly,
                Amount = 0, // Invalid amount
                ContributionDate = DateTime.UtcNow
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _contributionService.AddContributionAsync(contribution));
        }

        [Fact]
        public async Task CalculateInterest_ValidMember_ShouldCalculateInterest()
        {
            // Arrange
            var contributions = new List<Contribution>
            {
                new Contribution { MemberID = 1, Amount = 100 },
                new Contribution { MemberID = 1, Amount = 200 }
            };

            _mockContributionRepository
                .Setup(repo => repo.GetByMemberIdAsync(1))
                .ReturnsAsync(contributions);

            // Act
            await _contributionService.CalculateInterest(1);

            // Assert
            _mockContributionRepository.Verify(repo => repo.AddInterestAsync(1, 15m), Times.Once); // 5% of 300 = 15
        }

        [Fact]
        public async Task GetFailedTransactionsAsync_ReturnsFailedTransactions()
        {
            // Arrange
            var failedTransactions = new List<Contribution>
            {
                new Contribution { ContributionID = 1, IsValidated = false, ContributionDate = DateTime.UtcNow.AddDays(-1) },
                new Contribution { ContributionID = 2, IsValidated = false, ContributionDate = DateTime.UtcNow.AddDays(-2) }
            };

            _mockContributionRepository
                .Setup(repo => repo.GetFailedTransactionsAsync())
                .ReturnsAsync(failedTransactions);

            // Act
            var result = await _contributionService.GetFailedTransactionsAsync();

            // Assert
            Assert.Equal(2, result.Count);
            _mockContributionRepository.Verify(repo => repo.GetFailedTransactionsAsync(), Times.Once);
        }
    }
}
