using PensionContributionSystem.Model;
using PensionContributionSystem.Repository.Interface;
using PensionContributionSystem.Service.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PensionContributionSystem.Service.Implementation
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IContributionRepository _contributionRepository;

        public MemberService(
            IMemberRepository memberRepository,
            IContributionRepository contributionRepository)
        {
            _memberRepository = memberRepository;
            _contributionRepository = contributionRepository;
        }

        public async Task RegisterMember(Member member)
        {
            if (member.Age < 18 || member.Age > 70)
                throw new ArgumentException("Age must be between 18 and 70.");

            await _memberRepository.AddAsync(member);
        }

        public async Task UpdateMember(int memberId, Member member)
        {
            var existingMember = await _memberRepository.GetByIdAsync(memberId);
            if (existingMember == null)
                throw new KeyNotFoundException("Member not found.");

            existingMember.Name = member.Name;
            existingMember.Email = member.Email;
            existingMember.Phone = member.Phone;
            existingMember.UpdatedAt = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(existingMember);
        }

        public async Task<Member> GetMember(int memberId)
        {
            return await _memberRepository.GetByIdAsync(memberId);
        }

        public async Task SoftDeleteMember(int memberId)
        {
            var member = await _memberRepository.GetByIdAsync(memberId);
            if (member == null)
                throw new KeyNotFoundException("Member not found.");

            member.IsDeleted = true;
            await _memberRepository.UpdateAsync(member);
        }

        public async Task UpdateBenefitEligibility(int memberId)
        {
            var member = await _memberRepository.GetByIdAsync(memberId);
            if (member == null)
                throw new KeyNotFoundException("Member not found.");

            var contributions = await _contributionRepository.GetByMemberIdAsync(memberId);
            var totalMonthsContributed = contributions
                .GroupBy(c => new { c.ContributionDate.Year, c.ContributionDate.Month })
                .Count();

            // Minimum contribution period: 12 months
            if (totalMonthsContributed >= 12)
            {
                member.IsEligibleForBenefits = true;
                await _memberRepository.UpdateAsync(member);
            }
        }
    }
}