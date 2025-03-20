using Api.DTO_s;
using Api.Services.Interfaces;
using Data.Entities;

namespace Api.Services.Implementations
{
    public class NegotiationService : INegotiationService
    {
        private readonly Dictionary<int, Negotiation> _negotiations = new();
        private int _nextId = 1;

        public Negotiation CreateProposal(PriceProposalDTO proposalDto)
        {
            var existingNegotiations = _negotiations.Values
                .Where(n => n.ProductId == proposalDto.ProductId)
                .ToList();

            if (existingNegotiations.Any(n => n.Status == NegotiationStatus.Pending))
                throw new Exception("There is already a pending negotiation for this product");

            var attemptsCount = existingNegotiations.Count(n => n.Status == NegotiationStatus.Rejected);
            if (attemptsCount >= 3)
                throw new Exception("Maximum number of attempts reached");

            var lastRejected = existingNegotiations
                .Where(n => n.Status == NegotiationStatus.Rejected)
                .MaxBy(n => n.LastResponseDate);

            if (lastRejected != null && lastRejected.LastResponseDate.HasValue)
            {
                var daysSinceRejection = (DateTime.UtcNow - lastRejected.LastResponseDate.Value).TotalDays;
                if (daysSinceRejection > 7)
                    throw new Exception("Negotiation period has expired");
            }

            var negotiation = new Negotiation
            {
                Id = _nextId++,
                ProductId = proposalDto.ProductId,
                ProposedPrice = proposalDto.PriceProposed,
                ProposalDate = DateTime.UtcNow,
                Status = NegotiationStatus.Pending,
                AttemptsCount = attemptsCount + 1
            };

            _negotiations[negotiation.Id] = negotiation;
            return negotiation;
        }

        public Negotiation RespondToProposal(int negotiationId, bool accept)
        {
            if (!_negotiations.TryGetValue(negotiationId, out var negotiation))
                throw new Exception("Negotiation not found");

            if (negotiation.Status != NegotiationStatus.Pending)
                throw new Exception("Negotiation is not pending");

            negotiation.Status = accept ? NegotiationStatus.Accepted : NegotiationStatus.Rejected;
            negotiation.LastResponseDate = DateTime.UtcNow;

            return negotiation;
        }

        public Task<IEnumerable<Negotiation>> GetPendingNegotiations()
        {
            return Task.FromResult(_negotiations.Values.Where(n => n.Status == NegotiationStatus.Pending));
        }
    }
}