using System;

namespace TARE.Engine.Models
{
    [Flags]
    internal enum ObjectFlags
    {
        None = 0,
        CannotCarry = 1,
        Hidden,
    }
}
