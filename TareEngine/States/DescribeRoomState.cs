namespace TareEngine.States
{
    public class DescribeRoomState : DescribeRoomBaseState
    {
        public static DescribeRoomState Instance = new DescribeRoomState();

        private DescribeRoomState() : base(false)
        {
        }
    }
}
