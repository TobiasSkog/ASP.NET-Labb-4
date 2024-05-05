using Microsoft.EntityFrameworkCore;
using net23_asp_net_labb4_mvc.Models;

namespace net23_asp_net_labb4_mvc.Data;


public class DbInitializer(ApplicationDbContext _context)
{
    private static readonly Random _random = new();
    public async Task Initialize()
    {
        await _context.Database.EnsureCreatedAsync();

        if (_context.Customers.Any())
        {
            return;
        }


        // -- Create and Initialize Customers and add them to the DB -- //

        await InitCustomers();

        // -- Create and Initialize Books and add them to the DB -- //

        await InitBooks();

        // -- Create and Initialize BorrrowedBooks and add them to the DB -- //

        await InitBorrowedBook();
    }

    private async Task InitBooks()
    {
        var books = new List<Book>
        {
            new (){ Title = "The Great Gatsby", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "To Kill a Mockingbird", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "1984", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "Pride and Prejudice", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "The Catcher in the Rye", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "Animal Farm", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "Lord of the Flies", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "The Hobbit", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "The Grapes of Wrath", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "Frankenstein", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "Brave New World", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "The Chronicles of Narnia", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "The Lord of the Rings", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "The Adventures of Huckleberry Finn", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "Jane Eyre", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "Moby-Dick", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "The Picture of Dorian Gray", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "Crime and Punishment", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "Wuthering Heights", AmountInStock = _random.Next(3, 11) },
            new (){ Title = "One Hundred Years of Solitude", AmountInStock = _random.Next(3, 11) }
        };

        foreach (var book in books)
        {
            _context.Books.Add(book);
        }

        await _context.SaveChangesAsync();
    }

    private async Task InitCustomers()
    {
        var customers = new List<Customer>
        {
            new(){ Name = "Tobias Skog", Age = 31},
            new(){ Name = "Gustav Andersson", Age = 24},
            new(){ Name = "Karin Lindqvist", Age = 36},
            new(){ Name = "Elias Eriksson", Age = 49},
            new(){ Name = "Wilma Svensson", Age = 22},
            new(){ Name = "Hugo Nilsson", Age = 64},
            new(){ Name = "Emma Johansson", Age = 31},
            new(){ Name = "Liam Pettersson", Age = 28}
        };

        foreach (var customer in customers)
        {
            _context.Customers.Add(customer);
        }
        await _context.SaveChangesAsync();
    }


    private async Task InitBorrowedBook()
    {
        var borrowedBooks = new List<BorrowedBook>();

        var books = await _context.Books.ToListAsync();


        foreach (var customer in _context.Customers)
        {
            var numBooks = _random.Next(0, 12);
            Shuffle(books);
            var selectedBooks = books.Take(numBooks);

            foreach (var book in selectedBooks)
            {
                var randomBorrowingDate = GetRandomDateEarlierThanToday();
                borrowedBooks.Add(new BorrowedBook { CustomerId = customer.Id, BookId = book.Id, BorrowedDate = randomBorrowingDate });
            }
        }

        foreach (var borrowedBook in borrowedBooks)
        {
            await _context.AddAsync(borrowedBook);
        }

        await _context.SaveChangesAsync();
    }

    private static DateTime GetRandomDateEarlierThanToday()
    {
        var today = DateTime.Now;
        var randomPreviousDate = today.AddDays(_random.Next(0, 41));

        return randomPreviousDate;
    }

    private static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}