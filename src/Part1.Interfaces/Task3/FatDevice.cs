namespace Interfaces.Task3.Fat;

/// <summary>«Толстый» интерфейс: заставляет каждое устройство уметь всё сразу.</summary>
public interface IDevice
{
    void Print(string document);
    void Scan(string document);
    void Fax(string document);
}

public class Printer : IDevice
{
    public void Print(string document) => Console.WriteLine($"[Fat.Printer] печать: {document}");

    // Принтер не умеет сканировать и отправлять факс, но обязан объявить эти методы
    public void Scan(string document) { }
    public void Fax(string document) { }
}

public class Scanner : IDevice
{
    public void Scan(string document) => Console.WriteLine($"[Fat.Scanner] сканирование: {document}");

    // Сканер не умеет печатать и отправлять факс
    public void Print(string document) { }
    public void Fax(string document) { }
}
