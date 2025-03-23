using Microsoft.EntityFrameworkCore;
using PensionContributionSystem.Context;
using PensionContributionSystem.Model;
using PensionContributionSystem.Repository.Interface;

namespace PensionContributionSystem.Repository.Implementation
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDbContext _context;

        public MemberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Member member)
        {
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
        }

        public async Task<Member> GetByIdAsync(int memberId)
        {
            return await _context.Members.FindAsync(memberId);
        }

        public async Task UpdateAsync(Member member)
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Member>> GetAllAsync()
        {
            return await _context.Members.ToListAsync();
        }
    }
}
