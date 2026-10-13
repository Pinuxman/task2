namespace Interfaces.Task1;

public interface IMovable
{
    void Move(int x, int y);
}

public class Point(int x, int y) : IMovable
{
    public int X { get; private set; } = x;
    public int Y { get; private set; } = y;

    public void Move(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X}; {Y})";
}
