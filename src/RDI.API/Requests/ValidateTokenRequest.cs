using System;

namespace RDI.API.Requests
{
    public class ValidateTokenRequest
    {
        public int CustomerId { get; set; }

        public Guid Token { get; set; }

        public int CVV { get; set; }
    }
}