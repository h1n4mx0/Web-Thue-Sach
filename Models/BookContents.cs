using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManager.Models
{
public class BookContents
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Chapter { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int? PageStart { get; set; }
    public int? PageEnd { get; set; }
    public string Status { get; set; } = "Draft";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    [ForeignKey("BookId")]
    public virtual Books Book { get; set; }  
}
}
