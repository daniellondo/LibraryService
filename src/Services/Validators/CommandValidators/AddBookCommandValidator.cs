namespace Services.Validators.CommandValidators
{
    using System;
    using Domain.Dtos;
    using Domain.Entities;
    using FluentValidation;
    using Services.Validators.Shared;

    public class AddBookCommandValidator : AbstractValidator<AddBookCommand>
    {
        public AddBookCommandValidator(ICommonValidators commonValidators)
        {
            RuleFor(book => book.ISBN)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(isbn => !commonValidators.IsExistingEntityRow<Book>(u => u.ISBN == isbn))
                .WithMessage("You cannot add 2 books with the same ISBN");

            RuleFor(book => book.Pages).NotEmpty();
            RuleFor(book => book.Authors).NotEmpty();
            RuleFor(book => book.Synopsis).NotEmpty();
            RuleFor(book => book.Title).NotEmpty();
        }
    }
}
