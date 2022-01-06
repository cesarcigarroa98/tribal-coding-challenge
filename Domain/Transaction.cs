using System;

namespace Domain
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public String ClientIPAddress { get; set; }
        public DateTime? LastValidRequest { get; set; }
        public DateTime? LastInvalidRequest { get; set; }
        public int InvalidRequestsCounter { get; set; }
        public int RequestsCounter { get; set; }
        public double? Credit { get; set; }
    }
}
