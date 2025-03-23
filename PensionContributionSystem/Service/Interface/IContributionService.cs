using PensionContributionSystem.Model;

namespace PensionContributionSystem.Service.Interface
{
    public interface IContributionService
    {
        Task AddContributionAsync(Contribution contribution);
        Task<decimal> CalculateTotalContributionsAsync(int memberId);
        Task<string> GenerateStatementAsync(int memberId);
        Task UpdateContributionAsync(Contribution contribution);
        Task<List<Contribution>> GetUnvalidatedContributionsAsync();
        Task<List<Contribution>> GetFailedTransactionsAsync();
        Task CalculateInterest(int memberId);
    }
}
