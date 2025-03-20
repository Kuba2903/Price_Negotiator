using Api.DTO_s;
using Data.Entities;

namespace Api.Services.Interfaces
{
    public interface INegotiationService
    {
        Negotiation CreateProposal(PriceProposalDTO proposalDto);
        Negotiation RespondToProposal(int negotiationId, bool accept);
        Task<IEnumerable<Negotiation>> GetPendingNegotiations();
    }
}
