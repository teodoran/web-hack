namespace Notes.Api.AccessControl;

using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notes.Api.Database;
using Notes.Api.Models;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly NotesDb _database;

    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, NotesDb database)
        : base(options, logger, encoder, clock)
    {
        _database = database;
    }

    public static User GetUserFrom(string authorizationHeader)
    {
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(authorizationHeader);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);

            return new User
            {
                Username = credentials[0],
                Password = credentials[1],
            };
        }
        catch
        {
            return null;
        }
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }

        var authorizationHeader = Request.Headers["Authorization"];
        var authorizationUser = GetUserFrom(authorizationHeader);
        if (authorizationUser == null)
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        var storedUser = await _database.Users.FindAsync(authorizationUser.Username);
        if (storedUser == null || storedUser.Password != authorizationUser.Password)
        {
            return AuthenticateResult.Fail("Invalid Username or Password");
        }

        var ticket = GetAuthenticationTicket(storedUser);

        return AuthenticateResult.Success(ticket);
    }

    private AuthenticationTicket GetAuthenticationTicket(User user)
    {
        var claims = new Claim[] { new Claim(ClaimTypes.Name, user.Username) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);

        return new AuthenticationTicket(
            principal: new ClaimsPrincipal(identity),
            Scheme.Name);
    }
}