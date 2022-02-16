6 Vulnerable and Outdated Components
====================================
Resources:
- [Vulnerable and Outdated Components](https://owasp.org/Top10/A06_2021-Vulnerable_and_Outdated_Components/)
- [Log4Shell](https://nvd.nist.gov/vuln/detail/CVE-2021-44228)
- [Typosquatting](https://incolumitas.com/2016/06/08/typosquatting-package-managers/)
- [Dependency Confusion](https://medium.com/@alex.birsan/dependency-confusion-4a5d60fec610)

The Fault
---------
TODO

```json
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "Notes.Api": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "client",
      "applicationUrl": "http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ENABLE_VULNERABILITY": "true"
      }
    }
  }
}
```

From `GenerateRandom`.

```csharp
public static string Content =>
    new string("Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore."
        .Take(Random.Next(5, 100))
        .ToArray());
```

Not the `.ToArray()` we where looking for.

```csharp
namespace System.Linq;

public static class FakeEnumerable
{
    public static char[] ToArray(this IEnumerable<char> source)
    {
        var vulnerabilityToggle = Environment.GetEnvironmentVariable("ENABLE_VULNERABILITY");
        if (bool.TryParse(vulnerabilityToggle, out var enabled) && enabled)
        {
            Console.WriteLine($"VULNERABILITY ENABLED");
        }

        return System.Linq.Enumerable.ToArray(source);
    }
}
```

The Fix
-------
TODO

The Flag
--------
TODO