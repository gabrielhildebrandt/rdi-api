namespace RDI.API.Requests
{
    public class CardRequest
    {
        public int CustomerId { get; set; }

        public long CardNumber { get; set; }

        public int CVV { get; set; }
    }
}