using Library_systemEF.Model;
using Microsoft.EntityFrameworkCore;

namespace Library_systemEF.Repositories;

public class MemberRepository : IMemberRepositorie
{
    
    private readonly LibraryDbContext _context;

    public MemberRepository(LibraryDbContext context)
    {

        _context = context;

    }
    
    public async Task AddAsync(Member member)
        => await _context.Members.AddAsync(member);

    public async Task<Member?> CheckingEmailAsync(string email)
        => await _context.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Email == email);
    
    public async Task SaveAsync() => 
        await _context.SaveChangesAsync();

    public async Task DeleteId(int memberId)
    {
        var member = await _context.Members.FindAsync(memberId);
        if (member == null) return;
        _context.Members.Remove(member);


    }
        





}