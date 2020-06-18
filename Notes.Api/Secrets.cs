using System;

namespace Notes.Api
{
    public class Secrets
    {
        public string TestUserPassword { get; set; }

        [Obsolete("Removed this from the code long ago")]
        public string FirstFlag { get; set; }

        public string SecondFlag { get; set; }

        public string ThirdFlag { get; set; }

        public string FourthFlag { get; set; }

        public string FifthFlag { get; set; }

        public bool SeedData { get; set; }
    }
}
