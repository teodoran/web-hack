1 Sensitive Data Exposure
=========================
[Cryptographic failures](https://owasp.org/Top10/A02_2021-Cryptographic_Failures/) covers failures related to cryptography, or in this instance, a lack thereof. A symptom of cryptographic failure, is often sensitive data exposure, a broad topic that covers everything from exposing unsecured databases directly online to losing memory sticks with sensitive data. One common risk for developers, is to commit sensitive data alongside or embedded in the code to a public git-repository. Attackers regularly [search for exposed passwords at git hosting providers like GitHub](https://github.com/search?o=desc&q=filename%3Aappsettings.json+password&s=indexed&type=Code), and combined with [credential stuffing](https://owasp.org/www-community/attacks/Credential_stuffing), such a low-entry attack can be [quite devastating](https://en.wikipedia.org/wiki/Credential_stuffing#Incidents).

The Fault
---------
We have an application, but we don't have an account to login with. Let's start by finding one.

In .NET projects, configuration like database access keys and other passwords are usually stored in a file called [appsettings.json](../Notes.Api/appsettings.json). Seems like someone forgot to remove the password for the "Test"-user. Whoops!

The Fix
-------
The only good fix for exposed passwords and secrets is unfortunately to change them, and evaluate the potential loss of sensitive data. Since you don't have access to change the password used on the server, we unfortunately have to leave this issue as-is, and grind our teeth. You can of course change the password locally on your machine if you wish. If you do, you probably have to restart Notes.Api in order for the configuration change to take effect.

_There are several things you can do to reduce the chance of checking in passwords and other secrets. One is to make sure that all sensitive data is configurable from a development settings-file, like [appsettings.Development.json](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#default-configuration), that can be added to the .gitignore. Additional tips can be found at [GitHub](https://help.github.com/en/github/authenticating-to-github/removing-sensitive-data-from-a-repository#avoiding-accidental-commits-in-the-future)._

The Flag
--------
The first flag has been removed from the code long ago. But can you still find it?
