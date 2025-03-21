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


        /// <summary>
        /// Creates a new price negotiation proposal
        /// </summary>
        /// <param name="proposalDto">Price proposal data</param>
        /// <returns>Created negotiation</returns>
        /// <response code="201">Returns the newly created negotiation</response>
        /// <response code="400">If the proposal data is invalid</response>
        [HttpPost("proposals")]
        public async Task<ActionResult<Negotiation>> CreateProposal(PriceProposalDTO proposalDto)
        {
            ValidationResult result = await _validator.ValidateAsync(proposalDto);
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


        /// <summary>
        /// Responds to a price negotiation proposal (requires Employee role)
        /// </summary>
        /// <param name="id">Negotiation ID</param>
        /// <param name="accept">True to accept, false to reject the proposal</param>
        /// <returns>Updated negotiation</returns>
        /// <response code="200">Returns the updated negotiation</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="403">If user is not authorized as Employee</response>
        /// <response code="404">If negotiation is not found</response>
        [HttpPost("proposals/{id}/respond")]
        [Authorize(Roles = "Employee")]
        public ActionResult<Negotiation> RespondToProposal(int id, bool accept)
        {
            return _negotiationService.RespondToProposal(id, accept);
        }


        /// <summary>
        /// Gets all pending negotiations (requires Employee role)
        /// </summary>
        /// <returns>List of pending negotiations</returns>
        /// <response code="200">Returns list of pending negotiations</response>
        /// <response code="401">If user is not authenticated</response>
        /// <response code="403">If user is not authorized as Employee</response>
        [HttpGet("pending")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<Negotiation>>> GetPendingNegotiations()
        {
            return Ok(await _negotiationService.GetPendingNegotiations());
        }
    }
}
