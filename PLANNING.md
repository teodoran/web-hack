Web Hack: Planning
==================
_This is where @teodoran makes notes when planning the workshop._


Security vulnerabilities
------------------------
_These are the vulnerabilities that we're including in the workshop._

### 1. Sensitive Data Exposure
_In this hack we'll find the test username and password. Store some sensitive data i git log._

#### The Fault: Find some sensitive data in git log

References:
* [OWASP: Sensitive Data Exposure](https://owasp.org/www-project-top-ten/OWASP_Top_Ten_2017/Top_10-2017_A3-Sensitive_Data_Exposure)
* [GitHub: Removing sensitive data from a repository](https://help.github.com/en/github/authenticating-to-github/removing-sensitive-data-from-a-repository)

#### The Fix: Rotate secret by changing test-user password.

#### The Flag: Find some flag in log. There's a lot of logs, so you'll probably have to use a search command.


### 2. Broken Access Control
_In this hack we'll find notes for another user. Authenticated users can access other users resources._

#### The Fault: Retrieve notes from other user by changing URL.

Go over GET /notes/id until we find a note not made by current user.

References:
* [OWASP: Broken Access Control](https://owasp.org/www-project-top-ten/OWASP_Top_Ten_2017/Top_10-2017_A5-Broken_Access_Control)

#### The Fix: Check user ID as part of auth.

#### The Flag: Retrieve flag from a note from another user. The flag is injected on startup by accessing data from another API.


### 3. Cross Site Scripting (XSS)
_Inject some JS to gain information from other user._

#### The Fault: Example inject JS to own notes

This does not work. Probably due to browser built-in XXS-detection.
```html
<script>alert("XSS");</script>
```

This does work. Should reference OWASP XXS Filter Evasion Cheat Sheet.
```html
<svg/onload=alert('XSS')>
```

References:
* [OWASP: Cross Site Scripting (XSS)](https://owasp.org/www-project-top-ten/OWASP_Top_Ten_2017/Top_10-2017_A7-Cross-Site_Scripting_(XSS))
* [OWASP: XSS Filter Evasion Cheat Sheet](https://owasp.org/www-community/xss-filter-evasion-cheatsheet)

#### The Fix: Something with innerText vs. inner HTML.

Change `content.innerHTML = note.content;` to `content.innerText = note.content;`
Think about what's valid when validating in frontend and API. Strong validation helps.

#### The Flag: Some flag user works on notes. Flag user is triggered from external site.


### 4. SQL Injection
_Some SQL query to retrieve data that should not be available._

#### The Fault: Retrieve note from other user from DB.


Query parameter: `%' OR Author LIKE '%Sidney Wilson`
Query parameter: `%' OR Author LIKE '%`

References:
* [OWASP: Injection](https://owasp.org/www-project-top-ten/OWASP_Top_Ten_2017/Top_10-2017_A1-Injection)

#### The Fix: Use EF Core parameterized query.

```csharp
public Note[] Get([FromQuery] string containing) =>
    _database.Notes
        .Where(note => note.Author == user.Username)
        .Where(note => note.Content.Contains(containing))
        .OrderBy(note => note.Id)
        .ToArray();
```

#### The Flag: Retrieve flag som some other table in DB. Flag is injected from API external site.


### 5. Insecure Deserialization
_Running .NET code using insecure deserialization._

#### The Fault: Run some basic C# code on server.

PATCH /notes/...

```json
{
    "content": "Nå er denne oppdatert",
    "properties": {
        "hack": {
            "$type": "Notes.Api.Admin.Secret, Notes.Api",
            "name": "Teodor"
        }
    }
}
```

References:
* [OWASP: Insecure Deserialization](https://owasp.org/www-project-top-ten/OWASP_Top_Ten_2017/Top_10-2017_A8-Insecure_Deserialization)
* [How to configure Json.NET to create a vulnerable web API](https://www.alphabot.com/security/blog/2017/net/How-to-configure-Json.NET-to-create-a-vulnerable-web-API.html)
* [Friday the 13th JSON Attacks](https://www.blackhat.com/docs/us-17/thursday/us-17-Munoz-Friday-The-13th-JSON-Attacks-wp.pdf)

#### The Fix: Re-configure Json.NET.

```csharp
options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
```

But now deserialization probably don't work as well. Deserializing generics securely is hard.

Maybe do DTO?

#### The Flag: Retrieve flag from running .NET process. This is injected on launch by using value from other API.


Other vulnerabilities
---------------------
_Other vulnerabilities that we're skipping for now._

### Broken Authentication

Storing password or other secrets in browser?
- Find your own password and username in session storage?
- Extra: Find other users password and username in session storage?

### Insufficient Logging & Monitoring

Authentication module that doesn't log unsuccessful attempts?

### Using Components with Known Vulnerabilities

Custom unsecured component used?


Handling flags
--------------
Do something where API is configures by accessing data from external API?
Use Hashing to obfuscate data?
Make separate site with flags?
User-robot where you paste your API URL?
Will participants disable the fix before trying to find the flag?
