using PensionContributionSystem.Model;
using PensionContributionSystem.Repository.Interface;
using PensionContributionSystem.Service.Interface;
using System.Text;

namespace PensionContributionSystem.Service.Implementation
{
    public class ContributionService : IContributionService
    {
        private readonly IContributionRepository _contributionRepository;
        private readonly IMemberRepository _memberRepository;

        public ContributionService(
            IContributionRepository contributionRepository,
            IMemberRepository memberRepository)
        {
            _contributionRepository = contributionRepository;
            _memberRepository = memberRepository;
        }

        public async Task AddContributionAsync(Contribution contribution)
        {
            if (contribution.Amount <= 0)
                throw new ArgumentException("Contribution amount must be greater than 0.");

            if (contribution.ContributionDate > DateTime.UtcNow)
                throw new ArgumentException("Contribution date cannot be in the future.");

            await _contributionRepository.AddAsync(contribution);
        }

        public async Task<decimal> CalculateTotalContributionsAsync(int memberId)
        {
            var contributions = await _contributionRepository.GetByMemberIdAsync(memberId);
            return contributions.Sum(c => c.Amount);
        }

        public async Task<string> GenerateStatementAsync(int memberId)
        {
            var member = await _memberRepository.GetByIdAsync(memberId);
            if (member == null)
                throw new KeyNotFoundException("Member not found.");

            var contributions = await _contributionRepository.GetByMemberIdAsync(memberId);

            var statement = new StringBuilder();
            statement.AppendLine($"Contribution Statement for Member: {member.Name} (ID: {member.MemberID})");
            statement.AppendLine("==============================================");
            foreach (var contribution in contributions)
            {
                statement.AppendLine($"Date: {contribution.ContributionDate:yyyy-MM-dd}, Type: {contribution.ContributionType}, Amount: {contribution.Amount:C}");
            }
            statement.AppendLine("==============================================");
            statement.AppendLine($"Total Contributions: {contributions.Sum(c => c.Amount):C}");

            return statement.ToString();
        }

        public async Task UpdateContributionAsync(Contribution contribution)
        {
            if (contribution == null)
                throw new ArgumentNullException(nameof(contribution));

            await _contributionRepository.UpdateAsync(contribution);
        }

        public async Task<List<Contribution>> GetUnvalidatedContributionsAsync()
        {
            return await _contributionRepository.GetUnvalidatedContributionsAsync();
        }

        public async Task CalculateInterest(int memberId)
        {
            var contributions = await _contributionRepository.GetByMemberIdAsync(memberId);
            var totalContributions = contributions.Sum(c => c.Amount);

            decimal interestRate = 0.05m; // 5% annual interest
            decimal interestAmount = totalContributions * interestRate;

            // Save interest calculation
            await _contributionRepository.AddInterestAsync(memberId, interestAmount);
        }

        public async Task<List<Contribution>> GetFailedTransactionsAsync()
        {
            return await _contributionRepository.GetFailedTransactionsAsync();
        }
    }
}