namespace Notes.Api.Models;

using System;
using System.ComponentModel.DataAnnotations;
using Notes.Api.Configuration;

public class SubmitAnswer
{
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public Flag Flag { get; set; }

    [Required]
    public Guid Solution { get; set; }

    public bool IsValid(Secrets secrets)
    {
        return Flag switch
        {
#pragma warning disable 618
            Flag.SENSITIVE_DATA_EXPOSURE => Solution == secrets.SensitiveDataExposure,
#pragma warning restore 618
            Flag.BROKEN_ACCESS_CONTROL => Solution == secrets.BrokenAccessControl,
            Flag.CROSS_SITE_SCRIPTING => Solution == secrets.CrossSiteScripting,
            Flag.SQL_INJECTION => Solution == secrets.SqlInjection,
            Flag.INSECURE_DESERIALIZATION => Solution == secrets.InsecureDeserialization,
            Flag.VULNERABLE_AND_OUTDATED_COMPONENTS => Solution == secrets.VulnerableAndOutdatedComponents,
            _ => false,
        };
    }
}