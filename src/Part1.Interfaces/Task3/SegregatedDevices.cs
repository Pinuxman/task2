namespace Interfaces.Task3.Segregated;

public interface IPrinter
{
    void Print(string document);
}

public interface IScanner
{
    void Scan(string document);
}

public interface IFax
{
    void Fax(string document);
}

public class Printer : IPrinter
{
    public void Print(string document) => Console.WriteLine($"[Printer] печать: {document}");
}

public class Scanner : IScanner
{
    public void Scan(string document) => Console.WriteLine($"[Scanner] сканирование: {document}");
}

public class MultifunctionDevice : IPrinter, IScanner, IFax
{
    public void Print(string document) => Console.WriteLine($"[МФУ] печать: {document}");
    public void Scan(string document) => Console.WriteLine($"[МФУ] сканирование: {document}");
    public void Fax(string document) => Console.WriteLine($"[МФУ] факс: {document}");
}
