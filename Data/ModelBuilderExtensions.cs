using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using onlineBookstoreAPI.Models;
using System;

namespace onlineBookstoreAPI.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            // --- Users ---
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = "$2a$11$wQiGuG4trZtFqvbbYZ3RkOJROqEwlJ42Zm9qN3.Q/Rh2L/3K9Se4C\r\n", Role = "Admin" },
                new User { Id = 2, Username = "alice", PasswordHash = "$2a$11$LglINfhxwA3BzR5B7pKzWemWgxIxjVpvM8OAy1M0wFoB9PH7COH1K\r\n", Role = "User" },
                new User { Id = 3, Username = "bob", PasswordHash = "$2a$11$LglINfhxwA3BzR5B7pKzWemWgxIxjVpvM8OAy1M0wFoB9PH7COH1K\r\n", Role = "User" }
            );

            // --- Authors ---
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "George Orwell", Bio = "English novelist, essayist, journalist and critic." },
                new Author { Id = 2, Name = "Stephen Hawking", Bio = "Theoretical physicist, cosmologist, and author." },
                new Author { Id = 3, Name = "Yuval Noah Harari", Bio = "Historian and author of bestsellers." }
            );

            // --- Categories ---
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Science" },
                new Category { Id = 3, Name = "History" }
            );

            // --- Books ---
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "1984", Description = "Dystopian novel", Price = 9.99m, Stock = 100, ImageUrl = "/images/1984.jpg", Created = new DateTime(2025, 05, 12), AuthorId = 1, CategoryId = 1 },
                new Book { Id = 2, Title = "A Brief History of Time", Description = "Cosmology explained", Price = 15.50m, Stock = 50, ImageUrl = "/images/brief_history.jpg", Created = new DateTime(2025, 05, 12), AuthorId = 2, CategoryId = 2 },
                new Book { Id = 3, Title = "Sapiens", Description = "History of humankind", Price = 18.99m, Stock = 75, ImageUrl = "/images/sapiens.jpg", Created = new DateTime(2025, 05, 10), AuthorId = 3, CategoryId = 3 }
            );

            // --- Orders ---
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, OrderDate = new DateTime(2025,05,10), TotalAmount = 25.49m, UserId = 2 },
                new Order { Id = 2, OrderDate = new DateTime(2025,06,11), TotalAmount = 18.99m, UserId = 3 }
            );

            // --- OrderItems ---
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, OrderId = 1, BookId = 1, Quantity = 1, UnitPrice = 9.99m },
                new OrderItem { Id = 2, OrderId = 1, BookId = 2, Quantity = 1, UnitPrice = 15.50m },
                new OrderItem { Id = 3, OrderId = 2, BookId = 3, Quantity = 1, UnitPrice = 18.99m }
            );
        }
    }
}
