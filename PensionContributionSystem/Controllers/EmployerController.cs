using Microsoft.AspNetCore.Mvc;
using PensionContributionSystem.Model;
using PensionContributionSystem.Service.Interface;

namespace PensionContributionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployerController : ControllerBase
    {
        private readonly IEmployerService _employerService;

        public EmployerController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterEmployer([FromBody] Employer employer)
        {
            try
            {
                await _employerService.RegisterEmployer(employer);
                return Ok("Employer registered successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
