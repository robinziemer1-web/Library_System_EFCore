using System.Text.Json.Serialization;

namespace Library_systemEF.Model;

public class Book_Author
{
    [JsonPropertyName("bookId")]
    public int BookId { get; set; }
    public Book? Book { get; set; }
    [JsonPropertyName("authorId")]
    public int AuthorId { get; set; }
    public Author? Author { get; set; }


}