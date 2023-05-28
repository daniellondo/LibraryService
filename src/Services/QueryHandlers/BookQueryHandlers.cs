namespace Services.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using Domain.Dtos;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class BookQueryHandlers
    {
        public class GetBookByISBNQueryHandler : IRequestHandler<GetBookByISBNQuery, BaseResponse<BookDto>>
        {
            private readonly IContext _context;
            private readonly IMapper _mapper;

            public GetBookByISBNQueryHandler(IContext databaseContext, IMapper mapper)
            {
                _context = databaseContext;
                _mapper = mapper;
            }

            public async Task<BaseResponse<BookDto>> Handle(GetBookByISBNQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var book = await _context.Books
                                    .Include(x => x.Authors)
                                    .Include(x => x.Editorial)
                                    .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                                    .FirstOrDefaultAsync(x => x.ISBN.Equals(query.ISBN));

                    return new BaseResponse<BookDto>("", book);
                }
                catch (Exception ex)
                {
                    return new BaseResponse<BookDto>("Error getting data", null, ex);
                }

            }
        }

        public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, BaseResponse<List<BookDto>>>
        {
            private readonly IContext _context;
            private readonly IMapper _mapper;

            public GetBooksQueryHandler(IContext databaseContext, IMapper mapper)
            {
                _context = databaseContext;
                _mapper = mapper;
            }

            public async Task<BaseResponse<List<BookDto>>> Handle(GetBooksQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var books = await _context.Books
                                    .Include(x => x.Authors)
                                    .Include(x => x.Editorial)
                                    .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                                    .ToListAsync(cancellationToken);

                    return new BaseResponse<List<BookDto>>("", books);
                }
                catch (Exception ex)
                {
                    return new BaseResponse<List<BookDto>>("Error getting data", null, ex);
                }

            }
        }
    }
}
