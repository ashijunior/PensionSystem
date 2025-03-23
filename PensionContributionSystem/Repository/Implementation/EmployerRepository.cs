using PensionContributionSystem.Context;
using PensionContributionSystem.Model;
using PensionContributionSystem.Repository.Interface;

namespace PensionContributionSystem.Repository.Implementation
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly AppDbContext _context;

        public EmployerRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task AddAsync(Employer employer)
        {
            await _context.Employers.AddAsync(employer);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Employer> GetByIdAsync(int employerId)
        {
            return await _context.Employers.FindAsync(employerId);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Employer employer)
        {
            _context.Employers.Update(employer);
            await _context.SaveChangesAsync();
        }
    }
}
