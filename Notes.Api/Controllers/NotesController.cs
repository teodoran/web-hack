using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.Api.Database;
using Notes.Api.Models;

namespace Notes.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly NotesDb _database;

        public NotesController(NotesDb database)
        {
            _database = database;
        }

        /// <summary>
        /// Retrieves a list of all sticky notes written by a given author.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Note[] Get([FromQuery] QueryParameters parameters) =>
            _database.Notes
                .Where(note => note.Author == parameters.Author)
                .OrderBy(note => note.Id)
                .ToArray();

        /*[HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Note[] Get([FromQuery] QueryParameters parameters)
        {
            // TODO: Switch to SQLite to get FromSqlRaw to work
            return _database.Notes.FromSqlRaw("SELECT * FROM dbo.Notes").ToArray();
        }*/

        /// <summary>
        /// Creates a new sticky note.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Note> Post([FromBody] CreateNote createNote)
        {
            var note = new Note
            {
                Author = createNote.Author,
                Content = createNote.Content,
            };

            _database.Add(note);
            _database.SaveChanges();

            return CreatedAtRoute("GetNoteById", new { noteId = note.Id }, note);
        }

        /// <summary>
        /// Retrieves a single sticky note.
        /// </summary>
        [HttpGet("{noteId}", Name = "GetNoteById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Note> Get([FromRoute] int noteId)
        {
            var note = _database.Notes.Find(noteId);
            if (note == null)
            {
                return NotFound($"Note with noteId {noteId} not found");
            }

            return Ok(note);
        }

        /// <summary>
        /// Updates part of a single sticky note.
        /// </summary>
        [HttpPatch("{noteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Note> Patch([FromRoute] int noteId, [FromBody] UpdateNote patchNote)
        {
            var note = _database.Notes.Find(noteId);
            if (note == null)
            {
                return NotFound($"Note with noteId {noteId} not found");
            }

            note.Content = patchNote.Content;
            _database.SaveChanges();

            return Ok(note);
        }

        /// <summary>
        /// Deletes a single sticky note.
        /// </summary>
        [HttpDelete("{noteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Note> Delete([FromRoute] int noteId)
        {
            // This breaks the system.
            // if (noteId >= 0)
            // {
            //     throw new System.Exception("BOOM!");
            // }
            var note = _database.Notes.Find(noteId);
            if (note == null)
            {
                return NotFound($"Note with noteId {noteId} not found");
            }

            _database.Notes.Remove(note);
            _database.SaveChanges();

            return Ok();
        }
    }
}
