using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace net23_asp_net_labb4_mvc.Models;

public class BorrowedBook
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }
    public bool IsReturned { get; set; } = false;

    [Required]
    [DisplayName("Borrowed Date")]
    public DateTime BorrowedDate { get; set; }
    [DisplayName("Returned Date")]
    public DateTime? ReturnedDate { get; set; }
}
