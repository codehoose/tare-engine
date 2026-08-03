namespace TareEngine.Exceptions
{
    using System;

    internal class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message) { }
    }
}
