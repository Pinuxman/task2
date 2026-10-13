namespace UserApi;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string PassHash { get; set; } = string.Empty;
}

/// <summary>Тело запросов POST и PUT.</summary>
public record UserRequest(string? Login, string? PassHash);

/// <summary>Ответ с данными пользователя. Хеш пароля наружу не отдаётся.</summary>
public record UserResponse(int Id, string Login);

public record MessageResponse(string Message);
