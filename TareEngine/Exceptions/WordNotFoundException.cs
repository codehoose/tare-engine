namespace TareEngine.Exceptions
{
    using System;

    internal class WordNotFoundException : Exception
    {
        public WordNotFoundException(string message) : base(message) { }
    }
}
