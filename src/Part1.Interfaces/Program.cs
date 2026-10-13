using Interfaces.Task1;
using Interfaces.Task2;
using Interfaces.Task3.Segregated;
using Interfaces.Task4;
using Fat = Interfaces.Task3.Fat;

Console.WriteLine("=== Задание 1. Базовые интерфейсы ===");
var point = new Point(1, 2);
Console.WriteLine($"Точка до перемещения: {point}");
point.Move(10, -5);
Console.WriteLine($"Точка после Move(10, -5): {point}");

var drawables = new List<IDrawable> { new Circle(2), new Rectangle(3, 4), new Circle(0.5) };
Painter.DrawAll(drawables);

Console.WriteLine();
Console.WriteLine("=== Задание 2. Интерфейсы и наследование ===");
ShapePrinter.PrintShapeInfo(new Circle(5));
ShapePrinter.PrintShapeInfo(new Rectangle(3, 4));
ShapePrinter.PrintShapeInfo(new Cube(3));

Console.WriteLine();
Console.WriteLine("=== Задание 3. Сегрегация интерфейсов ===");
Console.WriteLine("До разделения (IDevice):");
Fat.IDevice fatPrinter = new Fat.Printer();
fatPrinter.Print("отчёт.docx");
fatPrinter.Scan("отчёт.docx"); // компилируется, но ничего не делает
Console.WriteLine("  вызов Scan у принтера ничего не сделал: метод пустой");

Console.WriteLine("После разделения (IPrinter, IScanner, IFax):");
IPrinter printer = new Printer();
IScanner scanner = new Scanner();
var mfu = new MultifunctionDevice();
printer.Print("отчёт.docx");
scanner.Scan("паспорт.pdf");
mfu.Print("договор.docx");
mfu.Scan("договор.docx");
mfu.Fax("договор.docx");

Console.WriteLine();
Console.WriteLine("=== Задание 4. Интерфейсы и полиморфизм ===");
PaymentProcessor.ProcessPayment(new CreditCard("4276000011112222", 5000m), 1250.50m);
PaymentProcessor.ProcessPayment(new Cash(), 300m);

Worker.DoWork(new ConsoleLogger());

var logPath = Path.Combine(Path.GetTempPath(), "lab2-work.log");
File.Delete(logPath);
Worker.DoWork(new FileLogger(logPath));
Console.WriteLine($"FileLogger записал в {Path.GetFileName(logPath)}:");
foreach (var line in File.ReadLines(logPath))
    Console.WriteLine($"  {line}");
