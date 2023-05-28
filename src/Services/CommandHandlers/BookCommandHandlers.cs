namespace Services.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data;
    using Domain.Dtos;
    using Domain.Entities;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class BookCommandHandlers
    {
        public class AddBookCommandHandler : IRequestHandler<AddBookCommand, BaseResponse<bool>>
        {
            private readonly IContext _context;
            private readonly IMapper _mapper;
            public AddBookCommandHandler(IContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<BaseResponse<bool>> Handle(AddBookCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var book = _mapper.Map<Book>(command);
                    book.Authors = CheckAuthors(book);
                    await CheckEditorial(book, cancellationToken);
                    await _context.Books.AddAsync(book, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    return new BaseResponse<bool>("Added successfully!", true);
                }
                catch (Exception ex)
                {
                    return new BaseResponse<bool>(ex.Message + " " + ex.StackTrace, false, StatusCodes.Status500InternalServerError);
                }
            }

            private List<Author> CheckAuthors(Book book)
            {
                var existingAuthors = new List<Author>();
                book.Authors.ForEach(author =>
                {
                    var authordb = _context.Authors.FirstOrDefault(at => at.Name == author.Name);
                    if (authordb is not null)
                    {
                        existingAuthors.Add(authordb);
                    }
                });

                var authors = book.Authors
                                .Where(la => !existingAuthors.Any(a => a.Name == la.Name && a.Surname == la.Surname))
                                .ToList();

                authors.AddRange(existingAuthors);
                return authors;
            }

            private async Task CheckEditorial(Book book, CancellationToken cancellationToken)
            {
                if (book.Editorial is not null)
                {
                    var editorial = await _context.Editorials
                        .Where(a => a.Branch == book.Editorial.Branch && a.Name == book.Editorial.Name)
                        .FirstOrDefaultAsync(cancellationToken);
                    if (editorial is not null)
                    {
                        book.Editorial = editorial;
                        book.EditorialId = editorial.Id;
                    }
                    else
                    {
                        book.EditorialId = book.Editorial.Id;
                    }
                }
            }
        }
    }
}
