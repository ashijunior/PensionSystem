using Microsoft.AspNetCore.Mvc;
using PensionContributionSystem.Model;
using PensionContributionSystem.Service.Interface;

namespace PensionContributionSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterMember([FromBody] Member member)
        {
            await _memberService.RegisterMember(member);
            return Ok();
        }

        [HttpPut("update/{memberId}")]
        public async Task<IActionResult> UpdateMember(int memberId, [FromBody] Member member)
        {
            await _memberService.UpdateMember(memberId, member);
            return Ok();
        }

        [HttpGet("{memberId}")]
        public async Task<ActionResult<Member>> GetMember(int memberId)
        {
            var member = await _memberService.GetMember(memberId);
            return Ok(member);
        }

        [HttpDelete("soft-delete/{memberId}")]
        public async Task<IActionResult> SoftDeleteMember(int memberId)
        {
            await _memberService.SoftDeleteMember(memberId);
            return Ok();
        }
    }
}
