namespace Notes.Api.Admin;

using System;
using System.Net.Http;
using Notes.Api.Configuration;

public class Secret
{
    private Uri _url;

    public static Secrets Secrets { get; set; }

    public string Name
    {
        get => "Not really important";
        set => Console.WriteLine($"Access granted {value}");
    }

    public Uri Url
    {
        get => _url;
        set => _url = value;
    }

    public object Client
    {
        get => null;
        set
        {
            var client = value as HttpClient;
            client.PostAsync(_url, new StringContent($"FLAG: {Secrets.InsecureDeserialization}")).GetAwaiter().GetResult();
        }
    }
}