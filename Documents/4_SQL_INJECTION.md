4 SQL Injection
===============
_Some SQL query to retrieve data that should not be available._

The Fault: Retrieve note from other user from DB
------------------------------------------------

Query parameter: `%' OR Author LIKE '%Sidney Wilson`
Query parameter: `%' OR Author LIKE '%`

References:
* [OWASP: Injection](https://owasp.org/www-project-top-ten/OWASP_Top_Ten_2017/Top_10-2017_A1-Injection)

The Fix: Use EF Core parameterized query
----------------------------------------

```csharp
public Note[] Get([FromQuery] string containing) =>
    _database.Notes
        .Where(note => note.Author == user.Username)
        .Where(note => note.Content.Contains(containing))
        .OrderBy(note => note.Id)
        .ToArray();
```

The Flag
--------
