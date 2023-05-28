namespace Services.MapperConfiguration
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Domain.Dtos;
    using Domain.Entities;

    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<AddBookCommand, Book>(MemberList.Source)
                .ForMember(book => book.Id, opt => Guid.NewGuid())
                .ForMember(book => book.Editorial, opt => opt.MapFrom(command => new Editorial
                {
                    Name = command.Editorial.Name,
                    Branch = command.Editorial.Branch,
                }))
                .ForMember(book => book.Authors, opt => opt.MapFrom(command => command.Authors.Select(autor => new Author
                {
                    Name = autor.Name,
                    Surname = autor.Surname
                })));
            CreateMap<Book, BookDto>(MemberList.Source);
            CreateMap<Editorial, EditorialDto>(MemberList.Source);
            CreateMap<Author, AuthorDto>(MemberList.Source);

        }
    }
}
