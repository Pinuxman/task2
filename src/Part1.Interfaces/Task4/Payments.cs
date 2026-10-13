namespace Interfaces.Task4;

public interface IPayable
{
    void Pay(decimal amount);
}

public class CreditCard(string number, decimal balance) : IPayable
{
    public decimal Balance { get; private set; } = balance;

    public void Pay(decimal amount)
    {
        if (amount > Balance)
            throw new InvalidOperationException("Недостаточно средств на карте");

        Balance -= amount;
        Console.WriteLine($"Оплата картой *{number[^4..]}: {amount:F2} руб., остаток {Balance:F2} руб.");
    }
}

public class Cash : IPayable
{
    public void Pay(decimal amount) => Console.WriteLine($"Оплата наличными: {amount:F2} руб.");
}

public static class PaymentProcessor
{
    public static void ProcessPayment(IPayable method, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Сумма должна быть больше нуля");

        method.Pay(amount);
    }
}
