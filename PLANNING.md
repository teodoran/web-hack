Web Hack: Planning
==================
_This is where @teodoran makes notes when planning the workshop._


Security vulnerabilities
------------------------
_These are the vulnerabilities that we're including in the workshop._

### Sensitive Data Exposure
_In this hack we'll find the test username and password._

Store some sensitive data i git log
- The Fault: Find some sensitive data in git log
- The Fix: Rotate secret by changing test-user password.
- The Flag: Find some flag in log. There's a lot of logs, so you'll probably have to use a search command.

### Broken Access Control
_In this hack we'll find notes for another user._

Authenticated users can access other users resources.
- The Fault: Retrieve notes from other user by changing URL.
- The Fix: Check user ID as part of auth.
- The Flag: Retrieve flag from a note from another user. The flag is injected on startup by accessing data from another API.

### Cross Site Scripting (XSS)

Inject some JS to gain information from other user.
- The Fault: Example inject JS to own notes

This does not work. Probably due to browser built-in XXS-detection.
```html
<script>alert("XSS");</script>
```

This does work. Should reference OWASP XXS Filter Evasion Cheat Sheet.
```html
<svg/onload=alert('XSS')>
```

- The Fix: Something with innerText vs. inner HTML.

Change `content.innerHTML = note.content;` to `content.innerText = note.content;`
Think about what's valid when validating in frontend and API. Strong validation helps.

- The Flag: Some flag user works on notes. Flag user is triggered from external site.

### SQL Injection

Some SQL query to retrieve data that should not be available.
- The Fault: Retrieve note from other user from DB.
- The Fix: Use EF Core parameterized query.
- The Flag: Retrieve flag som some other table in DB. Flag is injected from API external site.

### Insecure Deserialization

Configure Json.NET so that C#-code can be injected. https://www.alphabot.com/security/blog/2017/net/How-to-configure-Json.NET-to-create-a-vulnerable-web-API.html
- The Fault: Run some basic C# code on server.
- The Fix: Re-configure Json.NET.
- The Flag: Retrieve flag from running .NET process. This is injected on launch by using value from other API.


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
