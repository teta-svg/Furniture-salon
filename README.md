# Курсовая работа
# Furniture Salon

Приложение для управления мебельным салоном, разработанное на C# и .NET 8.

## О проекте

Furniture Salon — программное решение, предназначенное для автоматизации работы мебельного салона.

Проект разделён на несколько компонентов, что обеспечивает удобство разработки, сопровождения и тестирования приложения.

## Используемые технологии

- **Язык программирования:** C#
- **Платформа:** .NET 8
- **Настольное приложение:** WPF
- **Backend:** ASP.NET Core Web API
- **База данных:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **Тестирование:** xUnit

### Назначение проектов

| Проект | Описание |
|---|---|
| `FurnitureSalon.Api` | Серверная часть приложения, API и взаимодействие с базой данных |
| `FurnitureSalon.Application` | Настольное приложение на WPF |
| `FurnitureSalon.Core` | Общие компоненты |
| `FurnitureSalon.xUnit` | Автоматизированное тестирование компонентов приложения |

## Установка и запуск

### Клонирование репозитория

```bash
git clone https://github.com/teta-svg/Furniture-salon.git
cd Furniture-salon
```

### Сборка проекта

```bash
dotnet build Furniture-salon.slnx
```

### Запуск API

```bash
dotnet run --project src/FurnitureSalon.Api
```

### Запуск настольного приложения

```bash
dotnet run --project src/FurnitureSalon.Application
```

Для предварительной сборки приложения:

```bash
dotnet build src/FurnitureSalon.Application
```

После успешной сборки запустите приложение:

```bash
dotnet run --project src/FurnitureSalon.Application
```

### Запуск тестов

```bash
dotnet test
```
