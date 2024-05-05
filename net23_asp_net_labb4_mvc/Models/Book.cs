using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace net23_asp_net_labb4_mvc.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters")]
    public string Title { get; set; }

    [Required]
    [Range(0, 50, ErrorMessage = "Stock must be between 0 and 50")]
    [DisplayName("Amount In Stock")]
    public int AmountInStock { get; set; }

    [DisplayName("In Stock")]
    public bool InStock { get; set; } = true;

    public ICollection<BorrowedBook> BorrowedBooks { get; set; } = [];
}
