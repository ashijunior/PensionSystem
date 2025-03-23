using Microsoft.EntityFrameworkCore;
using PensionContributionSystem.Context;
using PensionContributionSystem.Model;
using PensionContributionSystem.Repository.Interface;
using System;

namespace PensionContributionSystem.Repository.Implementation
{
    public class ContributionRepository : IContributionRepository
    {
        private readonly AppDbContext _context;

        public ContributionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Contribution contribution)
        {
            await _context.Contributions.AddAsync(contribution);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Contribution>> GetByMemberIdAsync(int memberId)
        {
            return await _context.Contributions
                .Where(c => c.MemberID == memberId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Contribution contribution)
        {
            _context.Contributions.Update(contribution);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Contribution>> GetUnvalidatedContributionsAsync()
        {
            return await _context.Contributions
                .Where(c => !c.IsValidated)
                .ToListAsync();
        }

        public async Task AddInterestAsync(int memberId, decimal interestAmount)
        {
            var interestContribution = new Contribution
            {
                MemberID = memberId,
                ContributionType = ContributionType.Interest,
                Amount = interestAmount,
                ContributionDate = DateTime.UtcNow,
                IsValidated = true
            };

            await _context.Contributions.AddAsync(interestContribution);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Contribution>> GetFailedTransactionsAsync()
        {
            return await _context.Contributions
                .Where(c => !c.IsValidated && c.ContributionDate <= DateTime.UtcNow)
                .ToListAsync();
        }
    }
}
