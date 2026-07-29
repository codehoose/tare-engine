namespace TareEngine.Serialization
{
    [Serializable]
    public class CondActions
    {
        /// <summary>
        /// Pre condition-actions will be run before every command
        /// </summary>
        public SerializedFlagSet[] pre;

        /// <summary>
        /// Post condition-actions will be run after every command
        /// </summary>
        public SerializedFlagSet[] post;
    }
}
