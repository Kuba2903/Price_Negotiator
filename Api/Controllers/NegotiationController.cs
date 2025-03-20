using Api.DTO_s;
using Api.Services.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NegotiationController : ControllerBase
    {
        private readonly INegotiationService _negotiationService;

        public NegotiationController(INegotiationService negotiationService)
        {
            _negotiationService = negotiationService;
        }

        [HttpPost("proposals")]
        public ActionResult<Negotiation> CreateProposal(PriceProposalDTO proposalDto)
        {
            var negotiation = _negotiationService.CreateProposal(proposalDto);
            return CreatedAtAction(nameof(GetPendingNegotiations), new { id = negotiation.Id }, negotiation);
        }

        [HttpPost("proposals/{id}/respond")]
        //[Authorize(Roles = "Employee")]
        public ActionResult<Negotiation> RespondToProposal(int id, bool accept)
        {
            return _negotiationService.RespondToProposal(id, accept);
        }

        [HttpGet("pending")]
        //[Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<Negotiation>>> GetPendingNegotiations()
        {
            return Ok(await _negotiationService.GetPendingNegotiations());
        }
    }
}
