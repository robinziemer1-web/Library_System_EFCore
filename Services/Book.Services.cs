using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using Library_systemEF.Model;
using Library_systemEF.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Library_systemEF.Services;


public class Book_Services
{

    public static async Task BookListAsync()
    {

        await using var context = new LibraryDbContext();
        var repo = new BookRepository(context);
        
        var books = await repo.GetBookWithLoanAsync();

        foreach (var b in books)
        {

            bool isLoaned = b.Loans.Any(l => l.ReturnedAt == null);
            Console.WriteLine($"{b.Id,2}. {b.Title,-50} | {(isLoaned ? "On loan" : "Available" )}");
            
        }

    }

    public static async Task CreateBookWithAuthorAsync()
    {
        string firstName;
        string lastName;
        string title;
        string isbn;
        int year;
        
        await using var context = new LibraryDbContext();

        

        DateOnly birthDate;
        while (true)
        {

            Console.Clear();
            Console.WriteLine("----- Add a new book -----");
            Console.Write("Title:");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {

                Console.WriteLine("Title cant be empty, write something!");
                continue;
            }

            title = input.Trim();
           
            break;
        }

        while (true)
        {
            
            
            Console.Write("ISBN(only 13 chars): ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input) || input.Length != 13)
            {

                Console.WriteLine(" ISBN must have exactly 13 characters. Not less or more.");
                continue;

            }
 
            bool isbnExists = await context.Books.AnyAsync(b => b.ISBN == input);
            if (isbnExists)
            {

                Console.WriteLine("Theres already book with that ISBN!");
                continue;

            }

            isbn = input.Trim();

            break;

        }

        while (true)
        {
            
            Console.Write("Publication year: ");
            if (!int.TryParse(Console.ReadLine(), out year))
            {

                Console.WriteLine("Incorrect! Type with number.");
                continue;

            }

            break;


        }

        
            
        
            Console.WriteLine("----- Type in a Author for the book -----");

            while (true)
            {
                
                Console.Write("Authors first name: ");

                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) || !input.All(char.IsLetter))
                {

                    Console.WriteLine("Incorrect typing! Only letters are allowed.");
                    continue;
                }

                firstName = input.Trim();
                break;
            }

            while (true)
            {
                
                Console.Write("Authors last name: ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) || !input.All(char.IsLetter))
                {

                    Console.WriteLine("Incorrect typing! Only letters are allowed.");
                    continue;
                }

                lastName = input.Trim();
                break;
            }

            while (true)
            {
                
                Console.Write("Author's birth date (yyyy-MM-dd): ");
                var input = Console.ReadLine();

                if (!DateOnly.TryParse(input, out birthDate))
                {

                    Console.WriteLine("Invalid typing! You need to write exactly yyyy-MM-dd");
                    Console.WriteLine("For example: 1968-12-22");
                    continue;

                }

                if (birthDate > DateOnly.FromDateTime(DateTime.UtcNow))
                {

                    Console.WriteLine("Ey! You are not from the future! or.. are you a alien, time traveler?");
                    Console.WriteLine("Type again!");
                    continue;

                }
                
                

                break;

            }

            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {

                var author = new Author
                {

                    FirstName = firstName,
                    LastName = lastName,
                    BirthDay = birthDate


                };

                context.Authors.Add(author);
                await context.SaveChangesAsync();

                var book = new Book
                {

                    Title = title, // "!" means i know its not null. Cause validation before.
                    ISBN = isbn,
                    PublicationYear = year,
                    BookAddedLibrary = DateTime.UtcNow

                };

                context.Books.Add(book);
                await context.SaveChangesAsync();

                context.Set<Book_Author>().Add(new Book_Author()
                {

                    BookId = book.Id,
                    AuthorId = author.Id

                });

                await context.SaveChangesAsync();
                
                await transaction.CommitAsync();

                Console.WriteLine("Book and author for the book has now been created and linked togheter!");
                
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                Console.WriteLine("Error! something happend! Transaction has moved back!");
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();
            return;
    }

} 



