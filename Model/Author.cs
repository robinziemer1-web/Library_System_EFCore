using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_systemEF.Model;

public class Author
{
    
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = "";
    
    [Required]
    [MaxLength(100)]
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = "";
    [JsonPropertyName("birthDay")]
    public DateOnly BirthDay { get; set; }
     
    public ICollection<Book_Author> BookAuthors { get; set; } = new List<Book_Author>();
    
    

}