using BookStore.Domain.Abstractions;
using BookStore.Domain.Models;

namespace BookStore.Application.Services;

public class BooksService : IBooksService
{
    private readonly IBookRepository _bookRepository;

    public BooksService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public async Task<List<Book>> GetAllBooks()
    {
        return await _bookRepository.Get();
    }
    public async Task<Guid> CreateBook(Book book)
    {
        return await _bookRepository.Create(book);
    }
    public async Task<Guid> UpdateBook(Guid id, string title, string description, decimal price)
    {
        return await _bookRepository.Update(id, title, description, price);
    }
    public async Task<Guid> DeleteBook(Guid id)
    {
        return await _bookRepository.Delete(id);
    }
}
