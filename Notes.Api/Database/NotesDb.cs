namespace Notes.Api.Database;

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notes.Api.Configuration;
using Notes.Api.Models;

public class NotesDb : DbContext
{
    private readonly Secrets _secrets;

    public NotesDb() { }

    public NotesDb(DbContextOptions<NotesDb> options, IOptions<Secrets> secrets) : base(options)
    {
        _secrets = secrets.Value;
    }

    public void SeedData()
    {
        if (_secrets.SeedData)
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }
    }

    public DbSet<Note> Notes { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Secret> Secrets { get; set; }

    public DbSet<Answer> Answers { get; set; }

    public bool Contains(Answer answer) => Answers.Any(a => a.Name == answer.Name && a.Flag == answer.Flag);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (!_secrets.SeedData)
        {
            return;
        }

        var users = Enumerable
            .Range(0, 400)
            .Select(n => GenerateRandom.User)
            .GroupBy(p => p.Username)
            .Select(g => g.First())
            .ToList();

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

        var secrets = Enumerable
            .Range(1, 500)
            .Select(n => new Secret
            {
                Id = n,
                Value = Guid.NewGuid().ToString(),
            })
            .ToList();

        if (_secrets != null)
        {
            users.Add(new User { Username = "Test", Password = _secrets.TestUserPassword });
            notes.Add(new Note { Id = 1001, Author = "Test", Content = "Husk å kjøpe brød" });
            notes.Add(new Note { Id = 1002, Author = "Test", Content = "Hva var passordet til databasen igjen?" });

            var flagNote = notes[random.Next(0, notes.Count - 1)];
            flagNote.Content = $"You'll never find my secret FLAG: {_secrets.BrokenAccessControl}";

            var referenceNote = notes[42];
            referenceNote.Content = $"Where's the note? {flagNote.Id}";

            var flagSecret = secrets[random.Next(0, secrets.Count - 1)];
            flagSecret.Value = $"FLAG: {_secrets.SqlInjection}";

            users.Add(new User { Username = "Admin", Password = _secrets.CrossSiteScripting.ToString() });
            notes.Add(new Note { Id = 1003, Author = "Admin", Content = "Du er Admin!" });
        }

        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<Note>().HasData(notes);
        modelBuilder.Entity<Secret>().HasData(secrets);
    }
}