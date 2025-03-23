using Microsoft.Extensions.Logging;
using PensionContributionSystem.Repository.Interface;
using PensionContributionSystem.Service.Implementation;
using PensionContributionSystem.Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PensionContributionSystem.BackgroundJobs
{
    public class HangfireJobs
    {
        private readonly IContributionService _contributionService;
        private readonly IMemberRepository _memberRepository;
        private readonly IMemberService _memberService;
        private readonly ILogger<HangfireJobs> _logger;
        private readonly INotificationService _notificationService;

        public HangfireJobs(
            IContributionService contributionService,
            IMemberRepository memberRepository,
            IMemberService memberService,
            ILogger<HangfireJobs> logger,
            INotificationService notificationService)
        {
            _contributionService = contributionService;
            _memberRepository = memberRepository;
            _memberService = memberService;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task ValidateContributions()
        {
            try
            {
                _logger.LogInformation("Starting contribution validation job.");

                var unvalidatedContributions = await _contributionService.GetUnvalidatedContributionsAsync();

                foreach (var contribution in unvalidatedContributions)
                {
                    if (contribution.Amount > 0 && contribution.ContributionDate <= DateTime.UtcNow)
                    {
                        contribution.IsValidated = true;
                        await _contributionService.UpdateContributionAsync(contribution);
                        _logger.LogInformation($"Contribution {contribution.ContributionID} validated successfully.");
                    }
                    else
                    {
                        _logger.LogWarning($"Contribution {contribution.ContributionID} failed validation.");
                    }
                }

                _logger.LogInformation("Contribution validation job completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while validating contributions.");
                throw; // Re-throw to let Hangfire retry the job
            }
        }

        public async Task UpdateBenefitEligibility()
        {
            var members = await _memberRepository.GetAllAsync();

            foreach (var member in members)
            {
                await _memberService.UpdateBenefitEligibility(member.MemberID);
            }
        }

        public async Task CalculateInterest()
        {
            var members = await _memberRepository.GetAllAsync();

            foreach (var member in members)
            {
                await _contributionService.CalculateInterest(member.MemberID);
            }
        }

        public async Task HandleFailedTransactions()
        {
            var failedTransactions = await _contributionService.GetFailedTransactionsAsync();

            foreach (var transaction in failedTransactions)
            {
                try
                {
                    await _contributionService.AddContributionAsync(transaction);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to retry transaction {transaction.ContributionID}");
                    await _notificationService.SendNotificationAsync(transaction.MemberID, "Your contribution failed. Please try again.");
                }
            }
        }

        private async Task SendNotification(int memberId, string message)
        {
            var member = await _memberRepository.GetByIdAsync(memberId);
            if (member != null)
            {
                // Simulate sending an email or SMS
                _logger.LogInformation($"Notification sent to {member.Email}: {message}");
            }
        }
    }
}