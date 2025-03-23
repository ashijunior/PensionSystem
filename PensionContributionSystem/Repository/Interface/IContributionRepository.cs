using PensionContributionSystem.Model;

namespace PensionContributionSystem.Repository.Interface
{
    public interface IContributionRepository
    {
        Task AddAsync(Contribution contribution);
        Task<List<Contribution>> GetByMemberIdAsync(int memberId);
        Task UpdateAsync(Contribution contribution);
        Task<List<Contribution>> GetUnvalidatedContributionsAsync();
        Task AddInterestAsync(int memberId, decimal interestAmount);
        Task<List<Contribution>> GetFailedTransactionsAsync();
    }
}
