namespace RDI.API.Responses
{
    public class ValidateTokenResponse
    {
        public ValidateTokenResponse(bool validated)
        {
            Validated = validated;
        }

        public bool Validated { get; }
    }
}