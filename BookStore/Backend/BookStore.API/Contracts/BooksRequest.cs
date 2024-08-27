namespace BookStore.API.Contracts;

public record BooksRequest(
    string Title,
    string Description,
    decimal Price);