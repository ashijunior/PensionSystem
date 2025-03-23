using Microsoft.AspNetCore.Mvc;
using PensionContributionSystem.Model;
using PensionContributionSystem.Service.Interface;

namespace PensionContributionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContributionController : ControllerBase
    {
        private readonly IContributionService _contributionService;
        private readonly ILogger<ContributionController> _logger;

        public ContributionController(
            IContributionService contributionService,
            ILogger<ContributionController> logger)
        {
            _contributionService = contributionService;
            _logger = logger;
        }

        [HttpPost("monthly")]
        public async Task<IActionResult> AddMonthlyContribution([FromBody] Contribution contribution)
        {
            try
            {
                if (contribution.ContributionType != ContributionType.Monthly)
                    return BadRequest("Invalid contribution type. Expected Monthly.");

                await _contributionService.AddContributionAsync(contribution);
                return Ok("Monthly contribution added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a monthly contribution.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("voluntary")]
        public async Task<IActionResult> AddVoluntaryContribution([FromBody] Contribution contribution)
        {
            try
            {
                if (contribution.ContributionType != ContributionType.Voluntary)
                    return BadRequest("Invalid contribution type. Expected Voluntary.");

                await _contributionService.AddContributionAsync(contribution);
                return Ok("Voluntary contribution added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a voluntary contribution.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("total/{memberId}")]
        public async Task<IActionResult> GetTotalContributions(int memberId)
        {
            try
            {
                var totalContributions = await _contributionService.CalculateTotalContributionsAsync(memberId);
                return Ok(new { TotalContributions = totalContributions });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating total contributions.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("statement/{memberId}")]
        public async Task<IActionResult> GenerateStatement(int memberId)
        {
            try
            {
                var statement = await _contributionService.GenerateStatementAsync(memberId);
                return Ok(statement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating a contribution statement.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
