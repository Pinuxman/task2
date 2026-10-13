using Microsoft.EntityFrameworkCore;
using UserApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Users") ?? "Data Source=users.db"));

// Имена полей в JSON — как в задании: Id, Login, PassHash
builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.PropertyNamingPolicy = null);
builder.Services.Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = true);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<UserDbContext>().Database.EnsureCreated();

// Некорректный JSON в теле запроса — тоже ответ в формате JSON
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (BadHttpRequestException)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsJsonAsync(new MessageResponse("Тело запроса должно быть корректным JSON"));
    }
});

app.MapPost("/user", async (UserRequest request, UserDbContext db) =>
{
    if (UserValidator.Validate(request) is { } error)
        return Results.BadRequest(new MessageResponse(error));
    if (await db.Users.AnyAsync(u => u.Login == request.Login))
        return Results.Conflict(new MessageResponse($"Пользователь с логином {request.Login} уже существует"));

    var user = new User { Login = request.Login!, PassHash = request.PassHash!.ToLowerInvariant() };
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/user/{user.Id}", new UserResponse(user.Id, user.Login));
});

app.MapGet("/user/{id:int}", async (int id, UserDbContext db) =>
    await db.Users.FindAsync(id) is { } user
        ? Results.Ok(new UserResponse(user.Id, user.Login))
        : Results.NotFound(new MessageResponse($"Пользователь с Id={id} не найден")));

app.MapPut("/user/{id:int}", async (int id, UserRequest request, UserDbContext db) =>
{
    if (UserValidator.Validate(request) is { } error)
        return Results.BadRequest(new MessageResponse(error));

    var user = await db.Users.FindAsync(id);
    if (user is null)
        return Results.NotFound(new MessageResponse($"Пользователь с Id={id} не найден"));
    if (await db.Users.AnyAsync(u => u.Login == request.Login && u.Id != id))
        return Results.Conflict(new MessageResponse($"Пользователь с логином {request.Login} уже существует"));

    user.Login = request.Login!;
    user.PassHash = request.PassHash!.ToLowerInvariant();
    await db.SaveChangesAsync();
    return Results.Ok(new UserResponse(user.Id, user.Login));
});

app.MapDelete("/user/{id:int}", async (int id, UserDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null)
        return Results.NotFound(new MessageResponse($"Пользователь с Id={id} не найден"));

    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Ok(new MessageResponse($"Пользователь с Id={id} удалён"));
});

app.Run();

// Нужен интеграционным тестам (WebApplicationFactory<Program>)
public partial class Program;
