using Library_systemEF.Services;
using Microsoft.EntityFrameworkCore;
namespace Library_systemEF;

class Program
{
    public static async Task Main(string[] args)
    {
        
        

        using (var context = new LibraryDbContext())
        {
            
            DbSeeder.Seed(context);
            
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the library simulator!");
            Console.WriteLine("Choose a alternative: ");
            Console.WriteLine("1. Log in to Library System.");
            Console.WriteLine("2. Register a new member.");
            Console.WriteLine("3. Browse Library.");
            Console.WriteLine("0. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
            
                case "1":
                    
                    
                    //Logging in to System.
                    var member = await Member_Services.LoggInToLibrary();

                    if (member != null)
                    {

                        await Member_Services.MemberMenu(member);

                    }
                    else
                    {

                        Console.WriteLine("Login declined!");
                    
                    }
                
                    break;
                case "2":
                    await Member_Services.CreateMember();
                    break;
                case "3":
                    await BroseMenu();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Error! Invalid Choice!");
                    Console.ReadKey();
                    break;
            
            }

            
        }
        

    }

    public static async Task BroseMenu()
    {

        while (true)
        {
            
            Console.Clear();
            Console.WriteLine("------ Browse Library ------");
            Console.WriteLine("1. Browse all books");
            Console.WriteLine("2. Browse all members");
            Console.WriteLine("3. Add a book to library");
            Console.WriteLine("0. Back to main menu");

            var choice = Console.ReadLine();

            switch (choice)
            {
                
                case "1":
                    await Book_Services.BookListAsync();
                    Console.ReadKey();
                    break;
                
                case "2":
                    await Member_Services.BrowseAllMemberasync();
                    Console.ReadKey();
                    break;
                
                case "3":
                    await Book_Services.CreateBookWithAuthorAsync();
                    Console.ReadKey();
                    break;
                case "0":
                    return;
            }

        }
        
    }
}