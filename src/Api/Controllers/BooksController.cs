namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Domain.Dtos;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Add Book
        /// </summary>
        /// <param name="command"></param>
        /// <example>10</example>
        [HttpPost]
        [Route("AddBook")]
        public async Task<IActionResult> Post([FromBody] AddBookCommand command)
        {
            var result = await _mediator.Send(command);

            return result.Error is null ? Ok(result) : StatusCode((int)result.Error, result);
        }

        /// <summary>
        /// Get book details per book ISBN
        /// </summary>
        /// <param name="query"></param>
        [HttpGet]
        [Route("GetBookByISBN")]
        public async Task<IActionResult> GetBookByISBNQuery([FromQuery] GetBookByISBNQuery query)
        {
            var result = await _mediator.Send(query);

            return result.Error is null ? Ok(result) : StatusCode((int)result.Error, result);
        }

        /// <summary>
        /// Get all book details
        /// </summary>
        [HttpGet]
        [Route("GetBooks")]
        public async Task<IActionResult> GetBooks()
        {
            var result = await _mediator.Send(new GetBooksQuery());

            return result.Error is null ? Ok(result) : StatusCode((int)result.Error, result);
        }
    }
}
