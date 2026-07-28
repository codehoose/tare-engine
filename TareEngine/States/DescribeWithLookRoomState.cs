namespace TareEngine.States
{
    internal class DescribeWithLookRoomState : DescribeRoomBaseState
    {
        public static DescribeWithLookRoomState Instance = new DescribeWithLookRoomState();

        private DescribeWithLookRoomState() : base(false)
        {
        }
    }
}
