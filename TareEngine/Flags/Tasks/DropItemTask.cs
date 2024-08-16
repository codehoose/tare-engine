namespace TareEngine.Flags.Tasks
{
    public class DropItemTask : IActionTask
    {
        private readonly Engine _engine;
        private readonly string _itemSlug;

        public DropItemTask(Engine engine, string itemSlug)
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
