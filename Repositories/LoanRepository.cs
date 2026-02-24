using Library_systemEF.Model;
using Microsoft.EntityFrameworkCore;

namespace Library_systemEF.Repositories;

public class LoanRepository
{

    private readonly LibraryDbContext _context;

    public LoanRepository(LibraryDbContext context)
    {

        _context = context;

    }

    public async Task<bool> IsBookOnLoadAsync(int bookId) =>
        await _context.Loans
            .AnyAsync(l => l.BookId == bookId && l.ReturnedAt == null);

    public async Task<bool> BookExistAsync(int bookId) =>
        await _context.Books.AnyAsync(b => b.Id == bookId);

    public async Task AddAsync(Loan loan) =>
        await _context.Loans.AddAsync(loan);

    public async Task SaveAsync() =>
        await _context.SaveChangesAsync();

    public async Task<List<Loan>> GetLoansByMemberAsync(int memberId)
    {

        return await _context.Loans
            .AsNoTracking()                          
            .Include(l => l.Book)
            .Where(l => l.MemberId == memberId && l.ReturnedAt == null) 
            .OrderBy(l => l.DueDate)
            .ToListAsync();
        
    }

    public async Task<List<Loan>> GetActiveLoansByMemberAsync(int memberId)
    {

        return await _context.Loans
            .Include(l => l.Book)
            .Where(l => l.MemberId == memberId && l.ReturnedAt == null)
            .OrderBy(l => l.DueDate)
            .ToListAsync();

    }
    
    


}