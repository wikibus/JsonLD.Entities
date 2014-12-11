using System;

namespace JsonLD.Entities.Tests.Entities
{
    public class Book
    {
        public Uri Id { get; set; }

        public Person Author { get; set; }
    }
}
