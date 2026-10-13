using System.Text.RegularExpressions;

namespace UserApi;

public static partial class UserValidator
{
    [GeneratedRegex("^[A-Za-z0-9_.-]{3,50}$")]
    private static partial Regex LoginRegex();

    // Пароль должен приходить уже хешированным: SHA-256 в виде 64 шестнадцатеричных символов
    [GeneratedRegex("^[0-9a-fA-F]{64}$")]
    private static partial Regex Sha256Regex();

    /// <summary>Возвращает текст ошибки или null, если данные корректны.</summary>
    public static string? Validate(UserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login))
            return "Поле Login обязательно";
        if (!LoginRegex().IsMatch(request.Login))
            return "Login: от 3 до 50 символов, допустимы латинские буквы, цифры и знаки _ . -";
        if (string.IsNullOrWhiteSpace(request.PassHash))
            return "Поле PassHash обязательно";
        if (!Sha256Regex().IsMatch(request.PassHash))
            return "PassHash должен быть SHA-256 хешем пароля (64 шестнадцатеричных символа), а не самим паролем";
        return null;
    }
}
