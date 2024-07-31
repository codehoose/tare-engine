namespace TARE.Engine.Flags.Tasks
{
    internal class DropItemTask : IActionTask
    {
        private readonly TareEngine _engine;
        private readonly string _itemSlug;

        public DropItemTask(TareEngine engine, string itemSlug)
        {
            _engine = engine;
            _itemSlug = itemSlug;
        }

        public void Do()
        {
            _engine.CurrentRoom.Items.Add(_engine.GetItem(_itemSlug));
        }
    }
}
