using Moq;
using PensionContributionSystem.Model;
using PensionContributionSystem.Repository.Interface;
using PensionContributionSystem.Service.Implementation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PensionContributionSystemTest
{
    public class MemberServiceTests
    {
        private readonly Mock<IMemberRepository> _mockMemberRepository;
        private readonly Mock<IContributionRepository> _mockContributionRepository;
        private readonly MemberService _memberService;

        public MemberServiceTests()
        {
            _mockMemberRepository = new Mock<IMemberRepository>();
            _mockContributionRepository = new Mock<IContributionRepository>();

            // Initialize MemberService with both dependencies
            _memberService = new MemberService(
                _mockMemberRepository.Object,
                _mockContributionRepository.Object
            );
        }

        [Fact]
        public async Task RegisterMember_ValidMember_ShouldCallAddAsync()
        {
            // Arrange
            var member = new Member
            {
                Name = "John Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Phone = "1234567890"
            };

            // Act
            await _memberService.RegisterMember(member);

            // Assert
            _mockMemberRepository.Verify(repo => repo.AddAsync(member), Times.Once);
        }

        [Fact]
        public async Task RegisterMember_InvalidAge_ShouldThrowException()
        {
            // Arrange
            var member = new Member
            {
                Name = "John Doe",
                DateOfBirth = new DateTime(2010, 1, 1), // Age will be less than 18
                Email = "john.doe@example.com",
                Phone = "1234567890"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _memberService.RegisterMember(member));
        }

        [Fact]
        public async Task UpdateBenefitEligibility_ValidMember_ShouldUpdateEligibility()
        {
            // Arrange
            var member = new Member { MemberID = 1, IsEligibleForBenefits = false };
            var contributions = new List<Contribution>
            {
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-1) },
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-2) },
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-3) },
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-4) },
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-5) },
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-6) },
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-7) },
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-8) },
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-9) },
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-10) },
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-11) },
                new Contribution { MemberID = 1, ContributionDate = DateTime.UtcNow.AddMonths(-12) }
            };

            _mockMemberRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(member);

            _mockContributionRepository
                .Setup(repo => repo.GetByMemberIdAsync(1))
                .ReturnsAsync(contributions);

            // Act
            await _memberService.UpdateBenefitEligibility(1);

            // Assert
            Assert.True(member.IsEligibleForBenefits);
            _mockMemberRepository.Verify(repo => repo.UpdateAsync(member), Times.Once);
        }
    }
}