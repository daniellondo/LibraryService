namespace Test
{
    using System.Threading.Tasks;
    using AutoFixture;
    using Domain.Dtos;
    using FluentValidation.TestHelper;
    using NSubstitute;
    using NUnit.Framework;
    using Services.Validators.CommandValidators;
    using Services.Validators.QueryValidators;
    using Services.Validators.Shared;

    public class BookTests
    {
        public static readonly Fixture _fixture = new();
        private readonly AddBookCommandValidator _addBookCommandValidator;
        private readonly GetBookByISBNQueryValidator _getBookByISBNQueryValidator;
        private readonly ICommonValidators _commonValidator;

        public BookTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _commonValidator = Substitute.For<ICommonValidators>();
            _addBookCommandValidator = new AddBookCommandValidator(_commonValidator);
            _getBookByISBNQueryValidator = new GetBookByISBNQueryValidator(_commonValidator);
        }

        [Test]
        public async Task BookCommandHandlers_AddBookCommandHandler_Empty()
        {
            // Arrange
            var command = new AddBookCommand();

            // Act
            var result = await _addBookCommandValidator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveAnyValidationError();
        }

        [Test]
        public async Task BookCommandHandlers_AddBookCommandHandler_EditorialOptional()
        {
            // Arrange
            var command = _fixture.Build<AddBookCommand>()
                            .Without(p => p.Editorial)
                            .Create();

            // Act
            var result = await _addBookCommandValidator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public async Task BookCommandHandlers_AddBookCommandHandler_Books_Without_Authors()
        {
            // Arrange
            var command = _fixture.Build<AddBookCommand>()
                            .Without(p => p.Authors)
                            .Create();

            // Act
            var result = await _addBookCommandValidator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveAnyValidationError();
        }
    }
}