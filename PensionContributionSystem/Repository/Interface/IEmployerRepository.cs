using PensionContributionSystem.Model;

namespace PensionContributionSystem.Repository.Interface
{
    public interface IEmployerRepository
    {
        Task AddAsync(Employer employer);

        Task<Employer> GetByIdAsync(int employerId);

        Task UpdateAsync(Employer employer);
    }
}
