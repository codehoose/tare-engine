namespace TARE.States
{
    internal class DescribeWithLookRoomState : BaseDescribeRoomState
    {
        public static DescribeWithLookRoomState Instance = new DescribeWithLookRoomState();

        private DescribeWithLookRoomState() : base(false)
        {
        }
    }
}
