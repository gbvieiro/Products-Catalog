using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Interfaces;

namespace Products.Catalog.Application.Services.Books;

public class BooksAppService(
    IRepository<Book> bookRepository,
    IRepository<Stock> stocksRepository,
    IMapper mapper
) : IBooksAppService
{
    private readonly IRepository<Book> _bookRepository = bookRepository;
    private readonly IRepository<Stock> _stocksRepository = stocksRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> CreateAsync(BookDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var book = _mapper.Map<Book>(dto);
        await _bookRepository.CreateAsync(book);

        var newBookStock = new Stock(0, book.Id);
        await _stocksRepository.CreateAsync(newBookStock);

        return book.Id;
    }

    public async Task<BookDto?> ReadAsync(Guid id)
    {
        var book = await _bookRepository.ReadAsync(id);
        return book != null ? _mapper.Map<BookDto>(book) : default;
    }

    public async Task UpdateAsync(Guid id, BookDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        var entity = await _bookRepository.ReadAsync(id);
        if (entity is not null)
        {
            var book = _mapper.Map<Book>(dto);
            await _bookRepository.UpdateAsync(id, book);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var stocks = await _stocksRepository.FindAsync(string.Empty, 0, int.MaxValue);
        var stock = stocks.FirstOrDefault(s => s.BookId == id);

        var tasks = new List<Task>
        {
            _bookRepository.DeleteAsync(id)
        };

        if (stock != null)
        {
            tasks.Add(_stocksRepository.DeleteAsync(stock.Id));
        }

        await Task.WhenAll(tasks);
    }

    public async Task<IReadOnlyCollection<BookDto>> FindAsync(string filterText)
    {
        var books = await _bookRepository.FindAsync(filterText, 0, 100);
        return _mapper.Map<List<BookDto>>(books.ToList());
    }
}