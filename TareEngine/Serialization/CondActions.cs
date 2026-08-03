namespace TareEngine.Serialization
{
    using System;

    [Serializable]
    public class CondActions
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        /// <summary>
        /// Pre condition-actions will be run before every command
        /// </summary>
        public SerializedFlagSet[] pre;

        /// <summary>
        /// Post condition-actions will be run after every command
        /// </summary>
        public SerializedFlagSet[] post;
#pragma warning restore CS8618
    }
}
