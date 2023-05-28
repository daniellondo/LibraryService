namespace Services.Validators.QueryValidators
{
    using Domain.Dtos;
    using Domain.Entities;
    using FluentValidation;
    using Services.Validators.Shared;

    public class GetBookByISBNQueryValidator : AbstractValidator<GetBookByISBNQuery>
    {
        public GetBookByISBNQueryValidator(ICommonValidators commonValidators)
        {
            RuleFor(payload => payload.ISBN)
                .NotEmpty()
                .WithMessage("ISBN is required");

            RuleFor(payload => payload.ISBN)
                .Cascade(CascadeMode.Stop)
                .Must(isbn => commonValidators
                .IsExistingEntityRow<Book>(user => user.ISBN == isbn))
                .WithMessage("ISBN must be valid.")
                .When(payload => payload != null);
        }
    }
}
