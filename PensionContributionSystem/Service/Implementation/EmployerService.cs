using PensionContributionSystem.Model;
using PensionContributionSystem.Repository.Interface;
using PensionContributionSystem.Service.Interface;

namespace PensionContributionSystem.Service.Implementation
{
    public class EmployerService : IEmployerService
    {
        private readonly IEmployerRepository _employerRepository;

        public EmployerService(IEmployerRepository employerRepository)
        {
            _employerRepository = employerRepository;
        }

        public async Task RegisterEmployer(Employer employer)
        {
            if (string.IsNullOrEmpty(employer.CompanyName))
                throw new ArgumentException("Company name is required.");

            if (employer.IsActive == false)
                throw new ArgumentException("Employer must be active.");

            await _employerRepository.AddAsync(employer);
        }
    }
}
