using Interfaces.Task1;
using Interfaces.Task2;
using Interfaces.Task4;

namespace Lab2.Tests;

public class InterfacesTests
{
    [Fact]
    public void Point_Move_ChangesCoordinates()
    {
        var point = new Point(1, 2);

        point.Move(10, -5);

        Assert.Equal((10, -5), (point.X, point.Y));
    }

    [Fact]
    public void Shapes_ReturnAreaAndPerimeter()
    {
        IShape rectangle = new Rectangle(3, 4);
        IShape circle = new Circle(1);

        Assert.Equal(12, rectangle.GetArea());
        Assert.Equal(14, rectangle.GetPerimeter());
        Assert.Equal(Math.PI, circle.GetArea(), 10);
        Assert.Equal(2 * Math.PI, circle.GetPerimeter(), 10);
    }

    [Fact]
    public void Cube_IsShapeWithVolume()
    {
        I3DShape cube = new Cube(3);

        Assert.Equal(27, cube.GetVolume());
        Assert.Equal(54, cube.GetArea());
    }

    [Fact]
    public void CreditCard_Pay_ReducesBalance_AndRejectsOverdraft()
    {
        var card = new CreditCard("4276000011112222", 1000m);

        PaymentProcessor.ProcessPayment(card, 400m);

        Assert.Equal(600m, card.Balance);
        Assert.Throws<InvalidOperationException>(() => PaymentProcessor.ProcessPayment(card, 601m));
    }

    [Fact]
    public void FileLogger_WritesMessagesToFile()
    {
        var path = Path.GetTempFileName();

        Worker.DoWork(new FileLogger(path));

        Assert.Equal(3, File.ReadAllLines(path).Length);
        File.Delete(path);
    }
}
