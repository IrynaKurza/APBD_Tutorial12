using Tutorial5.DTOs;

namespace Tutorial5.Services;

public interface ITripDbService
{
    Task<List<BookWithAuthorsDto>> GetBooks();
}