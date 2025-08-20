# 📋 Task Management System (TMS)

> Простая, интуитивная и функциональная система управления задачами для распределённых команд, использующих гибкие методологии (Agile, Scrum, Kanban).  
> Разработана как дипломный проект.

---

## 🛠️ Технологии

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-6.0-purple?style=flat&logo=.net)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-6.0-red?style=flat)
![SQL Server](https://img.shields.io/badge/Database-SQL_Server-blue?style=flat&logo=microsoft-sql-server)
![Bootstrap 5](https://img.shields.io/badge/UI-Bootstrap_5-7952B3?style=flat&logo=bootstrap)

---

## 🎯 Ключевые особенности

- ✅ Минималистичный и интуитивный интерфейс (принципы юзабилити)
- 🧩 **Kanban-доска** с поддержкой drag-and-drop (Sortable.js)
- 👥 Назначение задач — **пользователям или командам**
- 🔐 Система ролей: **Администратор** и **Пользователь**
- 🎨 Персонализация: **светлая/тёмная тема**, настройка профиля
- 🔍 Фильтрация задач по **приоритету** и **исполнителю**
- 🔒 Безопасность: **ASP.NET Identity**, защита от CSRF
- 📱 Адаптивный дизайн на **Bootstrap 5**

---

## ⚙️ Технологический стек

| Категория         | Технология                          |
|------------------|-------------------------------------|
| **Backend**       | ASP.NET Core MVC 6.0, C#            |
| **Frontend**      | Razor Pages, Bootstrap 5, JavaScript, Sortable.js |
| **ORM**           | Entity Framework Core (Code First)  |
| **База данных**   | MS SQL Server                       |
| **Аутентификация**| ASP.NET Core Identity               |
| **IDE**           | Visual Studio 2022                  |

---

## 🗄️ Архитектура

Система построена по классической **трёхзвенной архитектуре**:
<img width="807" height="927" alt="image" src="https://github.com/user-attachments/assets/271501f2-9902-44fc-ac04-3560f4575273" />


## 📦 Установка и запуск

### Предварительные требования

- ✅ Установленный [.NET 6.0 SDK](https://dotnet.microsoft.com/download)
- ✅ Установленный **SQL Server** (подойдёт Express или LocalDB)
- ✅ (Рекомендуется) **Visual Studio 2022** с workload'ом *ASP.NET и веб-разработка*

---

### Шаги для запуска

1. **Клонируйте репозиторий:**

```bash
git clone https://github.com/StrafeStreiv/Task.git
cd Task

Настройте базу данных
Откройте проект в Visual Studio, затем в файле appsettings.json укажите свою строку подключения:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskManagementSysDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
