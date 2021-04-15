using System;

namespace RDI.API.Responses
{
    public class CardResponse
    {
        public CardResponse(DateTime creationDate, Guid token, Guid cardId)
        {
            CreationDate = creationDate;
            Token = token;
            CardId = cardId;
        }
        
        public DateTime CreationDate { get;  }

        public Guid Token { get;  }

        public Guid CardId { get;  }
    }
}