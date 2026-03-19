namespace Epsi.MazeCs
{
    public class Key : ICollectable
    {
        public bool IsPersistent => true;
        public int Points => 5;

        public override bool Equals(object? obj)
        {
            return obj is Key;
        }

        public override int GetHashCode()
        {
            return typeof(Key).GetHashCode();
        }
    }
}