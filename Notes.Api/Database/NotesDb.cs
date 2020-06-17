using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Notes.Api.Models;

namespace Notes.Api.Database
{
    public class NotesDb : DbContext
    {
        public NotesDb() { }

        public NotesDb(DbContextOptions<NotesDb> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var users = Enumerable
                .Range(0, 400)
                .Select(n => GenerateRandom.User)
                .GroupBy(p => p.Username)
                .Select(g => g.First())
                .ToList();

            users.Add(new User { Username = "Test", Password = "123" });

            modelBuilder.Entity<User>().HasData(users);

            var random = new Random();
            var notes = Enumerable
                .Range(1, 1000)
                .Select(n => new Note
                {
                    Id = n,
                    Author = users[random.Next(users.Count - 1)].Username,
                    Content = GenerateRandom.Content,
                })
                .ToList();

            notes.Add(new Note { Id = 1001, Author = "Test", Content = "Husk å kjøpe brød" });
            notes.Add(new Note { Id = 1002, Author = "Test", Content = "Hva var passordet til databasen igjen?" });

            modelBuilder.Entity<Note>().HasData(notes);
        }
    }
}
