using BookStore.Application.Services;
using BookStore.DataAccess;
using BookStore.DataAccess.Repositories;
using BookStore.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();    
    });
});
builder.Services.AddDbContext<BookStoreDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(BookStoreDbContext)));
});
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IBookRepository, BooksRepository>();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();
app.MapControllers();

app.Run();
