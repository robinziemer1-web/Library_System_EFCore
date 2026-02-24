namespace Library_systemEF.Model;

public class Loan
{
    public int LoanId { get; set; }
    public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReturnedAt { get; set; }
    public DateTime DueDate { get; set; } = DateTime.UtcNow;
    
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
}