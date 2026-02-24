using System.Reflection.Metadata.Ecma335;
using Library_systemEF.Model;
using Library_systemEF.Services;
using Library_systemEF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using static BCrypt.Net.BCrypt;

namespace Library_systemEF.Services;

public class Member_Services
{

    private readonly IMemberRepositorie _repo;

    public Member_Services(IMemberRepositorie repo)
    {

        _repo = repo;

    }
    
    public static async Task MemberMenu(Member loggedInMember)
    {

        while (true)
        {
            
            Console.Clear();
            Console.WriteLine("--------------------");
            Console.WriteLine($"You logged in {loggedInMember.Name}!");
            Console.WriteLine("Choose alternative: ");
            Console.WriteLine("1. Loan a book");
            Console.WriteLine("2. Check your book-loan list");
            Console.WriteLine("3. Return a book");
            Console.WriteLine("4. Delete your membership");
            Console.WriteLine("0. Exit to menu");

            var choice = Console.ReadLine();

            switch (choice)
            {
            
                case "1":
                    await Book_Services.BookListAsync();  //Show booklist (with loan status)
                    
                    Console.Write("Enter BookId(number) to borrow (0 to exit)");

                    var input = Console.ReadLine();

                    if (!int.TryParse(input, out int bookId))
                    {
                        Console.WriteLine("Error! Invalid input. Use numbers.");
                        break;
                    }

                    if (bookId == 0)
                    {
                        break;
                    }

                    await Loan_Services.BorrowBookAsync(loggedInMember.MemberId, bookId);
                    
                    break;
                case "2":

                    await Loan_Services.PrintMemberActiveLoanAsync(loggedInMember.MemberId);
                    
                    break;
                case "3":
                    await Loan_Services.MemberReturnBook(loggedInMember.MemberId);
                    break;
                case "4":
                    bool deleted = await DeleteMemberShipAsync(loggedInMember.MemberId);
                    if (deleted)
                    {

                        Console.WriteLine("Press a key to return to main menu.");
                        Console.ReadKey();
                        return;

                    }
                    break;
                case "0":
                    //Exit to menu
                    return;
            
            
            }

            Console.ReadKey();
            
        }
        
        

    }

    public static async Task<Member?> LoggInToLibrary()
    {

        while (true)
        { 
            
            Console.WriteLine("Email (or type 0 for exit):");
            var email = Console.ReadLine();

            if (email == "0")
                return null;

            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("-------------------");
                Console.WriteLine("Email cannot be empty, please write more.");
                continue;

            }
            
            await using var context = new LibraryDbContext();
            
            var repo = new MemberRepository(context);

            var member = await repo.CheckingEmailAsync(email);
            
            if (member == null)
            {

                Console.WriteLine("No member with that email!");
                continue;
            }
            
            Console.Write("Password: ");
            var password = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("-------------------");
                Console.WriteLine("Cant be empty. Please try again. ");
                continue;
            }

            bool correctPassword = Verify(password, member.PasswordHash);

            if (!correctPassword)
            {
                
                Console.WriteLine("-----------------");
                Console.WriteLine("Wrong password! ");
                continue;

            }

            Console.WriteLine($"Logging in {member.Name}");
            return member;

        }
        

        
    }

    public static async Task CreateMember()
    {

        string? name = null;
        string? email = null;
        string? password = null;
        while (true)
        {
            
            Console.WriteLine("Name: ");
           name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || !name.All(char.IsLetter))
            {

                Console.WriteLine("Error input! Write in letter only: ");
                continue;

            }

            break;

        }

        while (true)
        {
            
            Console.WriteLine("Email: ");
            email = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(email))
            {

                Console.WriteLine("Incorrect input. You cant have empty or space in email. Try again.");
                continue;
            }

            break;
        }

        while (true)
        {
            
            Console.Write("Password: ");
            password = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {

                Console.WriteLine("You need to type atleast 6 characters. ");
                continue;
            
            }

            break;
        }
        
        string passwordHash = HashPassword(password!);;
        

       await using var context = new LibraryDbContext();

        var member = new Member
        {

            Name = name,
            Email = email,
            PasswordHash = passwordHash


        };
        
        var repo = new MemberRepository(context);

        await repo.AddAsync(member);

        

        Console.WriteLine($"Membern har skapats: {name}");

        await repo.SaveAsync();


    }

    public static async Task<bool> DeleteMemberShipAsync(int memberId)
    {

        Console.WriteLine("Are you sure to delete your membership?(y/n):");

        var answer = Console.ReadLine();

        if (answer != "y")
        {

            Console.WriteLine("Declined..");
            Console.WriteLine("Press a key to return to member menu.");
            Console.ReadKey();
            return false;

        }
        
       await using var context = new LibraryDbContext();

        var repo = new LoanRepository(context);

        var checkingMemberLoan = await repo.GetActiveLoansByMemberAsync(memberId);

        if (checkingMemberLoan.Any())
        {

            Console.WriteLine("You cant delete your account because you have active loans.");
            Console.WriteLine("Press a key to return to member menu.");
            return false;

        }

        var memberRepo = new MemberRepository(context);

       await memberRepo.DeleteId(memberId);

       await memberRepo.SaveAsync();

       Console.WriteLine("Your account has been deleted!");
       Console.WriteLine("Press a key to return to member menu.");
       Console.ReadKey();
       return true;
    }

    public static async Task BrowseAllMemberasync()
    {

        await using var context = new LibraryDbContext();

        var members = await context.Members
            .AsNoTracking()
            .Select(m => new
            {

                m.MemberId,
                m.Name,
                ActiveLoans = m.Loans.Count(l => l.ReturnedAt == null)

            }).ToListAsync();

        if (!members.Any())
        {

            Console.WriteLine("There are no members in library.");
            Console.WriteLine("Press a key to return to member menu.");
            Console.ReadKey();
            return;

        }

        foreach (var m in members)
        {

            Console.WriteLine($"{m.MemberId}. {m.Name,-10} - Active book loans : {m.ActiveLoans}");
            
        }

        Console.WriteLine("Press a key to return to member menu.");
        Console.ReadKey();


    }
    
    
    
}