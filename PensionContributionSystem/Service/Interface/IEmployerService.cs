using PensionContributionSystem.Model;

namespace PensionContributionSystem.Service.Interface
{
    public interface IEmployerService
    {
        Task RegisterEmployer(Employer employer);
    }
}
