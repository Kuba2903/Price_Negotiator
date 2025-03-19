using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Negotiation
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal ProposedPrice { get; set; }
        public DateTime ProposalDate { get; set; }
        public NegotiationStatus Status { get; set; }
        public int AttemptsCount { get; set; }
        public DateTime? LastResponseDate { get; set; }
    }

    public enum NegotiationStatus
    {
        Pending,
        Accepted,
        Rejected,
        Cancelled
    }
}
