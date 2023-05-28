namespace Domain.Dtos
{
    public class GetBookByISBNQuery : QueryBase<BaseResponse<BookDto>>
    {
        public long ISBN { get; set; }
    }
}
