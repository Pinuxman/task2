namespace Interfaces.Task1;

public interface IDrawable
{
    void Draw();
}

public static class Painter
{
    /// <summary>Рисует все фигуры, не зная их конкретных типов.</summary>
    public static void DrawAll(List<IDrawable> shapes)
    {
        foreach (var shape in shapes)
            shape.Draw();
    }
}
