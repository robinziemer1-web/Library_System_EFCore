using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;

namespace Library_systemEF.Model;

public class Book
{
    
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    [JsonPropertyName("title")]
    public string Title { get; set; } = "";
    
    [Required]
    [MaxLength(13)]
    [JsonPropertyName("isbn")]
    public string ISBN { get; set; } = "";
    [JsonPropertyName("publicationYear")]
    public int PublicationYear { get; set; }
    [JsonPropertyName("bookAddedLibrary")]
    public DateTime BookAddedLibrary { get; set; } = DateTime.UtcNow;

    public ICollection<Book_Author> BookAuthors { get; set; } = new List<Book_Author>();

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();

}