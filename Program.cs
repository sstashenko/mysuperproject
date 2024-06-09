using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    public class Book : IComparable<Book>
    {
        public string Title; // Назва книги
        public string Description; // Опис книги
        public string Author; // Імя автора
        public string Publisher; // Видавництво
        public int YearPub; // Рік публікації
        public int Pages; // Кількість сторінок
        public int Cost; // Ціна
        public bool TypeBinding;  // Перепліт істина - твердий, хибність - м'який

        public int CompareTo(Book other)
        {
            if (other == null) return 1;
            return this.YearPub.CompareTo(other.YearPub);
        }

        public override string ToString()
        {
            return $"Title: {Title}, Author: {Author}, Publisher: {Publisher}, Year: {YearPub}, Pages: {Pages}, Cost: {Cost}, Binding: {(TypeBinding ? "Hard" : "Soft")}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Book> books = new List<Book>
            {
                new Book { Title = "To Kill a Mockingbird", Description = "A novel about the injustices of the American South.", Author = "Harper Lee", Publisher = "J.B. Lippincott & Co.", YearPub = 1960, Pages = 281, Cost = 10, TypeBinding = true },
                new Book { Title = "1984", Description = "A dystopian novel set in a totalitarian society.", Author = "George Orwell", Publisher = "Secker & Warburg", YearPub = 1949, Pages = 328, Cost = 15, TypeBinding = false },
                new Book { Title = "Moby-Dick", Description = "The story of Captain Ahab's quest to avenge the whale that 'reaped' his leg.", Author = "Herman Melville", Publisher = "Harper & Brothers", YearPub = 1851, Pages = 635, Cost = 20, TypeBinding = true },
                new Book { Title = "Pride and Prejudice", Description = "A romantic novel that also critiques the British landed gentry.", Author = "Jane Austen", Publisher = "T. Egerton", YearPub = 1813, Pages = 279, Cost = 12, TypeBinding = false },
                new Book { Title = "The Great Gatsby", Description = "A novel about the American dream and the roaring twenties.", Author = "F. Scott Fitzgerald", Publisher = "Charles Scribner's Sons", YearPub = 1925, Pages = 180, Cost = 14, TypeBinding = true }
            };

            // Create and write to the file
            using (FileStream fs = new FileStream("Books.dat", FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                foreach (var book in books)
                {
                    writer.Write(book.Title);
                    writer.Write(book.Description);
                    writer.Write(book.Author);
                    writer.Write(book.Publisher);
                    writer.Write(book.YearPub);
                    writer.Write(book.Pages);
                    writer.Write(book.Cost);
                    writer.Write(book.TypeBinding);
                }
            }

            // Reading from the file
            books.Clear();
            using (FileStream fs = new FileStream("Books.dat", FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                try
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        Book book = new Book
                        {
                            Title = reader.ReadString(),
                            Description = reader.ReadString(),
                            Author = reader.ReadString(),
                            Publisher = reader.ReadString(),
                            YearPub = reader.ReadInt32(),
                            Pages = reader.ReadInt32(),
                            Cost = reader.ReadInt32(),
                            TypeBinding = reader.ReadBoolean()
                        };

                        books.Add(book);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading from file: {0}", ex.Message);
                }
            }

            // Display unsorted list of books
            Console.WriteLine("Unsorted list of books:");
            PrintBooks(books);

            // Sort books
            books.Sort();

            // Display sorted list of books
            Console.WriteLine("\nSorted list of books:");
            PrintBooks(books);

            // Add a new book to the collection
            Book newBook = new Book
            {
                Title = "Brave New World",
                Description = "A dystopian social science fiction novel.",
                Author = "Aldous Huxley",
                Publisher = "Chatto & Windus",
                YearPub = 1932,
                Pages = 311,
                Cost = 18,
                TypeBinding = true
            };
            books.Add(newBook);

            // Display sorted list with the new book added
            Console.WriteLine("\nSorted list with the new book added:");
            books.Sort(); // Sort again after adding new book
            PrintBooks(books);

            // Remove a book from the collection (e.g., the last book)
            if (books.Count > 0)
            {
                books.RemoveAt(books.Count - 1);
            }

            // Display the final list of books
            Console.WriteLine("\nFinal list after removing the last book:");
            PrintBooks(books);

            // Wait for user input before closing
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        public static void PrintBooks(List<Book> books)
        {
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }
    }
}
