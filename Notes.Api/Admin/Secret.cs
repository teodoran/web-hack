using System;

namespace Notes.Api.Admin
{
    public class Secret
    {
        public string Name
        {
            get => "Not really important";
            set => Console.WriteLine($"Access granted {value}");
        }
    }
}
