using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace net23_asp_net_labb4_mvc.Models;

public class Customer
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
    public string Name { get; set; }

    [Required]
    [Range(0, 120, ErrorMessage = "Age must be between 0 and 120")]
    public int Age { get; set; }


    [DisplayName("Borrowed Books")]
    public ICollection<BorrowedBook> BorrowedBooks { get; set; } = [];
}
