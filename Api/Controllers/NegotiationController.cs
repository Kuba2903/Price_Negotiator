using Api.DTO_s;
using Api.Services.Interfaces;
using Data.Entities;
using FluentValidation;
using FluentValidation.Results;
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
        private readonly IValidator<PriceProposalDTO> _validator;
        public NegotiationController(INegotiationService negotiationService, IValidator<PriceProposalDTO> validator)
        {
            _negotiationService = negotiationService;
            _validator = validator;
        }

        [HttpPost("proposals")]
        public ActionResult<Negotiation> CreateProposal(PriceProposalDTO proposalDto)
        {
            ValidationResult result = _validator.Validate(proposalDto);
            if (result.IsValid)
            {
                var negotiation = _negotiationService.CreateProposal(proposalDto);
                return CreatedAtAction(nameof(GetPendingNegotiations), new { id = negotiation.Id }, negotiation);
            }
            else
            {
                return BadRequest(result.Errors);
            }
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
