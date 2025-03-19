using Api.DTO_s;
using Data.Entities;

namespace Api.Services.Interfaces
{
    public interface INegotiationService
    {
        Task<Negotiation> CreateProposal(PriceProposalDTO proposalDto);
        Task<Negotiation> RespondToProposal(int negotiationId, bool accept);
        Task<IEnumerable<Negotiation>> GetPendingNegotiations();
    }
}
