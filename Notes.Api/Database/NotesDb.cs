using System;
using Microsoft.EntityFrameworkCore;
using Notes.Api.Models;

namespace Notes.Api.Database
{
    public class NotesDb : DbContext
    {
        public NotesDb() { }

        public NotesDb(DbContextOptions<NotesDb> options) : base(options) { }

        public DbSet<Note> Notes { get; set; }

        internal object Select()
        {
            throw new NotImplementedException();
        }
    }
}
