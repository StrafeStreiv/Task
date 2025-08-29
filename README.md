# Task Management System (TMS)

> Простая, интуитивная и функциональная система управления задачами для распределённых команд, использующих гибкие методологии (Agile, Scrum, Kanban).  


---

## Технологии

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-6.0-purple?style=flat&logo=.net)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-6.0-red?style=flat)
![SQL Server](https://img.shields.io/badge/Database-SQL_Server-blue?style=flat&logo=microsoft-sql-server)
![Bootstrap 5](https://img.shields.io/badge/UI-Bootstrap_5-7952B3?style=flat&logo=bootstrap)

---

## Ключевые особенности

-  Минималистичный и интуитивный интерфейс (принципы юзабилити)
-  **Kanban-доска** с поддержкой drag-and-drop (Sortable.js)
-  Назначение задач — **пользователям или командам**
-  Система ролей: **Администратор** и **Пользователь**
-  Персонализация: **светлая/тёмная тема**, настройка профиля
-  Фильтрация задач по **приоритету** и **исполнителю**
-  Безопасность: **ASP.NET Identity**, защита от CSRF
-  Адаптивный дизайн на **Bootstrap 5**

---

##  Технологический стек

| Категория         | Технология                          |
|------------------|-------------------------------------|
| **Backend**       | ASP.NET Core MVC 6.0, C#            |
| **Frontend**      | Razor Pages, Bootstrap 5, JavaScript, Sortable.js |
| **ORM**           | Entity Framework Core (Code First)  |
| **База данных**   | MS SQL Server                       |
| **Аутентификация**| ASP.NET Core Identity               |
| **IDE**           | Visual Studio 2022                  |

---

##  Архитектура

Система построена по классической **трёхзвенной архитектуре**:
<img width="807" height="927" alt="image" src="https://github.com/user-attachments/assets/271501f2-9902-44fc-ac04-3560f4575273" />


##  Установка и запуск

### Предварительные требования

- ✅ Установленный [.NET 6.0 SDK](https://dotnet.microsoft.com/download)
- ✅ Установленный **SQL Server** (подойдёт Express или LocalDB)
- ✅ (Рекомендуется) **Visual Studio 2022** с workload'ом *ASP.NET и веб-разработка*

---

### Шаги для запуска

**Клонируйте репозиторий:**

```bash
git clone https://github.com/StrafeStreiv/Task.git
cd Task


---
```
**Настройка базы данных**

1. Откройте проект в **Visual Studio 2022**.

2. Перейдите к файлу конфигурации `appsettings.json` и найдите раздел `ConnectionStrings`.

3. Обновите строку подключения `DefaultConnection`, указав ваш сервер и имя базы данных.

#### Пример для **LocalDB**:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskManagementSysDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```


---

###  Применение миграций

После настройки подключения необходимо создать базу данных и применить миграции.

1. Откройте **Package Manager Console** в Visual Studio:  
   `Tools` → `NuGet Package Manager` → `Package Manager Console`

2. Выполните команду:
```bash
Update-Database
```

✅ Это создаст все таблицы, соответствующие моделям приложения (включая таблицы Identity).

> ⚠️ Убедитесь, что выбран правильный проект по умолчанию в PMC.

---

###  Запуск приложения

Вы можете запустить приложение одним из способов:

- Нажмите **F5** в Visual Studio — запустится отладка с IIS Express.
- Или в терминале выполните:
```bash
dotnet run
```

 Приложение откроется по адресу:  
 [https://localhost:7000](https://localhost:7000)  
или [http://localhost:5000](http://localhost:5000)

---

###  Назначение администратора

При первом запуске система не содержит пользователей с ролью **Admin**. Чтобы получить доступ к админ-панели:

1. Зарегистрируйте учётную запись через форму регистрации.
2. Остановите приложение.
3. В **Package Manager Console** выполните SQL-запрос:

```sql
USE TaskManagementSysDb;
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.Email = 'your@email.com' AND r.Name = 'Admin';
```

>  Замените `your@email.com` на email зарегистрированного пользователя.

4. Перезапустите приложение.  
   Теперь в навигационной панели появится пункт **"Админ-панель"**.

---

