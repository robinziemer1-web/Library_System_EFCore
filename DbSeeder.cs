using System.Text.Json;
using Library_systemEF.Model;

namespace Library_systemEF;

public class DbSeeder
{

    public static void Seed(LibraryDbContext context)
    {

        if (context.Books.Any() || context.Authors.Any() || context.Set<Book_Author>().Any()) 
            return;

        
        
        var authorsJSON = File.ReadAllText("Data-seeds/Authors-seed.json");

        

        var authors = JsonSerializer.Deserialize<List<Author>>(authorsJSON);
        
        context.Authors.AddRange(authors);
        context.SaveChanges();

        var booksJSON = File.ReadAllText("Data-seeds/Books-seed.json");

        var books = JsonSerializer.Deserialize<List<Book>>(booksJSON);
        
        context.Books.AddRange(books);
        context.SaveChanges();

        var book_authorsJSON = File.ReadAllText("Data-seeds/Book_Authors-seed.json");

        var book_authors = JsonSerializer.Deserialize<List<Book_Author>>(book_authorsJSON);
        
        context.Set<Book_Author>().AddRange(book_authors);
        context.SaveChanges();
        
        


    }
    
    
}