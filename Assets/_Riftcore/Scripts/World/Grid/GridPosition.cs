namespace Riftcore.World.Grid
{
    public readonly struct GridPosition
    {
        private readonly int _x;
        private readonly int _y;

        public GridPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public override int GetHashCode()
        {
            return _x * 73856093 ^ _y * 19349663;
        }

        public override bool Equals(object obj)
        {
            return obj is GridPosition other && other._x == _x && other._y == _y;
        }
    }
}