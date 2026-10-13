using Interfaces.Task1;

namespace Interfaces.Task2;

public interface IShape
{
    double GetArea();
    double GetPerimeter();
}

/// <summary>Объёмная фигура: наследует контракт плоской и добавляет объём.</summary>
public interface I3DShape : IShape
{
    double GetVolume();
}

public class Circle(double radius) : IDrawable, IShape
{
    public double Radius { get; } = radius;

    public void Draw() => Console.WriteLine($"Рисую круг радиусом {Radius}");
    public double GetArea() => Math.PI * Radius * Radius;
    public double GetPerimeter() => 2 * Math.PI * Radius;
}

public class Rectangle(double width, double height) : IDrawable, IShape
{
    public double Width { get; } = width;
    public double Height { get; } = height;

    public void Draw() => Console.WriteLine($"Рисую прямоугольник {Width} x {Height}");
    public double GetArea() => Width * Height;
    public double GetPerimeter() => 2 * (Width + Height);
}

public class Cube(double edge) : I3DShape
{
    public double Edge { get; } = edge;

    /// <summary>Площадь полной поверхности куба.</summary>
    public double GetArea() => 6 * Edge * Edge;

    /// <summary>Сумма длин всех двенадцати рёбер.</summary>
    public double GetPerimeter() => 12 * Edge;

    public double GetVolume() => Edge * Edge * Edge;
}

public static class ShapePrinter
{
    public static void PrintShapeInfo(IShape shape)
    {
        Console.WriteLine($"{shape.GetType().Name}: площадь = {shape.GetArea():F2}, периметр = {shape.GetPerimeter():F2}");
        if (shape is I3DShape solid)
            Console.WriteLine($"{shape.GetType().Name}: объём = {solid.GetVolume():F2}");
    }
}
