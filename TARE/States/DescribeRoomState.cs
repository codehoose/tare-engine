namespace TARE.States
{
    internal class DescribeRoomState : BaseDescribeRoomState
    {
        public static DescribeRoomState Instance = new DescribeRoomState();

        private DescribeRoomState() : base(false)
        { 
        }
    }
}
