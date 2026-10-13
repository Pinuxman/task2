# Лабораторный практикум №2. Интерфейсы. Репозиторий. JSON

Решение на C# / .NET 10. Три части — три группы проектов.

| Часть | Проект | Содержание |
|---|---|---|
| 1. Интерфейсы | `src/Part1.Interfaces` | `IMovable`, `IDrawable`, `IShape`, `I3DShape`, разделение `IDevice` на `IPrinter`/`IScanner`/`IFax`, `IPayable`, `ILogger` |
| 2. Репозиторий | `src/Part2.BookStore.Data` | Библиотека: сущности книжного магазина, EF Core + SQLite, `IRepository<T>`, `GenericRepository<T>`, специализированные репозитории, async-методы, Unit of Work |
| | `src/Part2.BookStore.Demo` | Консольное приложение, использующее библиотеку |
| 3. JSON | `src/Part3.UserApi` | REST API: CRUD таблицы `User` (`POST/GET/PUT/DELETE /user`), JSON, валидация |
| Тесты | `tests/Lab2.Tests` | 20 тестов xUnit на все три части |

## Запуск

```bash
dotnet test
dotnet run --project src/Part1.Interfaces
dotnet run --project src/Part2.BookStore.Demo
dotnet run --project src/Part3.UserApi
```

API поднимается на http://localhost:5270, примеры запросов — в `src/Part3.UserApi/UserApi.http`.
Пароль передаётся только в виде SHA-256 хеша (64 шестнадцатеричных символа):

```bash
printf 'password123' | shasum -a 256
```
