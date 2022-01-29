namespace Notes.Api.Controllers;

using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Notes.Api.Configuration;
using Notes.Api.Database;
using Notes.Api.Models;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AnswersController : ControllerBase
{
    private readonly NotesDb _database;
    private readonly Secrets _secrets;

    public AnswersController(NotesDb database, IOptions<Secrets> secrets)
    {
        _database = database;
        _secrets = secrets.Value;
    }

    /// <summary>
    /// Retrieves a list of all answers.
    /// </summary>
    [HttpGet("", Name = "GetAnswers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public Answer[] Get() => _database.Answers.ToArray();

    /// <summary>
    /// Submits a new answer.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<Answer[]> Post([FromBody] SubmitAnswer submitAnswer)
    {
        if (!submitAnswer.IsValid(_secrets))
        {
            return BadRequest($"{submitAnswer.Solution} is not a valid solution for {submitAnswer.Flag}");
        }

        var answer = new Answer
        {
            Name = submitAnswer.Name.Trim().ToUpper(),
            Flag = submitAnswer.Flag,
        };

        if (_database.Contains(answer))
        {
            return Ok(_database.Answers.ToArray());
        }

        _database.Add(answer);
        _database.SaveChanges();

        return CreatedAtRoute("GetAnswers", _database.Answers.ToArray());
    }
}