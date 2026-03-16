namespace Epsi.MazeCs
{
    public class Coin : ICollectable
    {
        public bool IsPersistent => false;
        public int Points => 10;
    }
}