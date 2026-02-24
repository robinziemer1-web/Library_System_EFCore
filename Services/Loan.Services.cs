using Library_systemEF.Model;
using Library_systemEF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library_systemEF.Services;

public class Loan_Services
{

    public static async Task BorrowBookAsync(int memberId, int bookId)
    {

        await using var context = new LibraryDbContext();
        var repo = new LoanRepository(context);

        if (!await repo.BookExistAsync(bookId))
        {

            Console.WriteLine("Theres no book with that id!");
            return;

        }

        if (await repo.IsBookOnLoadAsync(bookId))
        {

            Console.WriteLine("Book is already borrowed. You need to wait for that!");
            return;

        }

        var loan = new Loan
        {

            MemberId = memberId,
            BookId = bookId,
            BorrowedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14),
            ReturnedAt = null

        };

        await repo.AddAsync(loan);
        await repo.SaveAsync();

        Console.WriteLine("Book is borrowed!");
        Console.WriteLine("Press a key to return to member menu.");
        Console.ReadKey();


    }

    public static async Task PrintMemberActiveLoanAsync(int memberId)
    {

        await using var context = new LibraryDbContext();

        var repo = new LoanRepository(context);

        var loans = await repo.GetLoansByMemberAsync(memberId);

        if (loans.Count == 0)
        {

            Console.WriteLine("You have no active loans!");
            Console.WriteLine("Press a key to return to member menu.");
            Console.ReadKey();
            return;

        }

        foreach (var loan in loans)
        {
            var timeLeft = loan.DueDate - DateTime.UtcNow;

            Console.WriteLine($"{loan.Book.Id,2}: {loan.Book.Title,-20}: Due : {loan.DueDate:yyyy-MM-dd}" + 
                              $"| ({timeLeft.Days} days {timeLeft.Hours} hours) left to return the book.");
            
        }
        Console.WriteLine("Press a key to return to member menu.");
        Console.ReadKey();

    }

    public static async Task MemberReturnBook(int memberId)
    {

        await using var context = new LibraryDbContext();

        var repo = new LoanRepository(context);

        var loans = await repo.GetActiveLoansByMemberAsync(memberId);

        

        if (!loans.Any())
        {

            Console.WriteLine("Theres no active loans in this account.");
            return;
        }

        foreach (var l in loans)
        {
            Console.WriteLine("----- Your active loaned books ------");
            Console.WriteLine($"{l.Book.Id,2}. {l.Book.Title}");
            
        }
        
        Console.WriteLine("Type a bookId(number) you want to return(0 for exit)");
        
        var input = Console.ReadLine();

        if (!int.TryParse(input, out var bookId))
        {

            Console.WriteLine("Incorrect! Use number.");
            
        }

        if (bookId == 0)
            return;

        var loanToReturn = loans.FirstOrDefault(l => l.BookId == bookId);

        if (loanToReturn == null)
        {

            Console.WriteLine("That book is not in your loan list.");
            return;
        }

        loanToReturn.ReturnedAt = DateTime.UtcNow;
        await repo.SaveAsync();

        Console.WriteLine("Book has returned succesfully!");




    }
    
}