namespace Notes.Api.Configuration;

using System;

public class Secrets
{
    public string TestUserPassword { get; set; }

    [Obsolete("Removed this from the code long ago")]
    public Guid SensitiveDataExposure { get; set; }

    public Guid BrokenAccessControl { get; set; }

    public Guid CrossSiteScripting { get; set; }

    public Guid SqlInjection { get; set; }

    public Guid InsecureDeserialization { get; set; }

    public Guid VulnerableAndOutdatedComponents { get; set; }

    public bool SeedData { get; set; }
}