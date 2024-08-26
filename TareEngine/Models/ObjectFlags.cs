namespace TareEngine.Models
{
    [Flags]
    public enum ObjectFlags
    {
        None = 0,
        CannotCarry = 1,
        Hidden,
        Openable,
    }
}
