namespace Library_systemEF.Model;

public class Member
{
    public int MemberId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}