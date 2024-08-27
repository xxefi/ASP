using BookStore.Domain.Models;

namespace BookStore.Domain.Abstractions;

public interface IBookRepository
{
    Task<Guid> Create(Book book);
    Task<Guid> Delete(Guid id);
    Task<List<Book>> Get();
    Task<Guid> Update(Guid id, string title, string description, decimal price);
}