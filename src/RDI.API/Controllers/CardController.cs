using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RDI.API.Requests;
using RDI.API.Responses;
using RDI.Application.Commands.CreateCard;
using RDI.Application.Commands.ValidateCardToken;
using RDI.Domain.Kernel;

namespace RDI.API.Controllers
{
    [Route("v1/cards")]
    public class CardController : Controller
    {
        public CardController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        private readonly IMediator _mediator;

        [HttpPost("")]
        [ProducesResponseType(typeof(CardResponse), (int) HttpStatusCode.Created)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CardRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest();

            var commandInput = new CreateCardCommandInput(request.CustomerId, request.CardNumber, request.CVV);
            var commandResult = await _mediator.Send(commandInput, cancellationToken);

            if (!commandResult.IsValid())
                return BadRequest();

            var response = new CardResponse(commandResult.CreationDate, commandResult.Token, commandResult.CardId);

            return Created("", response);
        }

        [HttpPost("{cardId:guid}")]
        [ProducesResponseType(typeof(ValidateTokenResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ValidateToken([FromRoute] Guid cardId, [FromBody] ValidateTokenRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
                return BadRequest();

            var commandInput = new ValidateCardTokenCommandInput(request.CustomerId, cardId, request.Token, request.CVV);
            var commandResult = await _mediator.Send(commandInput, cancellationToken);

            if (!commandResult.IsValid())
                return BadRequest();

            var response = new ValidateTokenResponse(commandResult.Validated);
            return Ok(response);
        }
    }
}