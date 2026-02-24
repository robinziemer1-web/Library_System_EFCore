using Library_systemEF.Model;
using Microsoft.EntityFrameworkCore;

namespace Library_systemEF.Repositories;

public class BookRepository
{
    
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {

        _context = context;

    }

    public async Task<List<Book>> GetBookWithLoanAsync()
        => await _context.Books
            .AsNoTracking()
            .Include(b => b.Loans)
            .ToListAsync();
    
    

}