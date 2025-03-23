using PensionContributionSystem.Model;

namespace PensionContributionSystem.Repository.Interface
{
    public interface IMemberRepository
    {
        Task AddAsync(Member member);
        Task<Member> GetByIdAsync(int memberId);
        Task UpdateAsync(Member member);
        Task<List<Member>> GetAllAsync();
    }
}
