# URL Shortener

Тестовое задание: веб-приложение для сокращения URL.

## Стек
- .NET 10
- ASP.NET Core MVC
- MariaDB
- MySqlConnector
- ADO.NET

## Что реализовано
- создание коротких ссылок
- список всех ссылок на главной странице
- переход по короткой ссылке
- подсчёт количества переходов
- редактирование длинной ссылки
- удаление ссылки
- валидация URL
- SQL-миграция для создания таблицы

## Структура
- `Controllers` — контроллеры приложения
- `Models` — модель сущности
- `Services` — генератор коротких кодов и репозиторий
- `ViewModels` — модели для представлений
- `Views` — Razor-представления
- `sql/001_init.sql` — SQL-скрипт инициализации базы

## Настройка
1. Создать базу данных MariaDB:
```sql
CREATE DATABASE urlshortener CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

2. Выполнить SQL-скрипт:
```sql
sql/001_init.sql
```

3. Указать строку подключения в `appsettings.json`

Пример:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=urlshortener;User ID=user;Password=your_password;"
}
```

## Запуск
```bash
dotnet restore
dotnet run
```

## Примечания по реализации
- Короткие коды генерируются случайно, а не последовательно, чтобы их было сложнее предсказать.
- Для подсчёта переходов используется прямой `UPDATE` в базе, чтобы логика редиректа оставалась простой.
- Для данного тестового проекта работа с MariaDB реализована через `MySqlConnector` и SQL-запросы.
