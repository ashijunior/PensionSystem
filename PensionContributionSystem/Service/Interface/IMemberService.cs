using PensionContributionSystem.Model;

namespace PensionContributionSystem.Service.Interface
{
    public interface IMemberService
    {
        Task RegisterMember(Member member);
        Task UpdateMember(int memberId, Member member);
        Task<Member> GetMember(int memberId);
        Task SoftDeleteMember(int memberId);
        Task UpdateBenefitEligibility(int memberId);
    }
}
