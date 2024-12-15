# ShopSolution

**ShopSolution** – это консольное приложение, имитирующее работу магазина. Пользователь может создавать магазины и товары, управлять их запасами, находить оптимальные цены и совершать покупки.

---

## 🚀 Функционал

- **Создание магазинов** с уникальными кодами, названиями и адресами.
- **Добавление товаров** в общий каталог.
- **Завоз товаров в магазины**: установка цены и количества.
- **Поиск оптимальных цен**: нахождение магазина с минимальной стоимостью определенного товара.
- **Расчет покупок на бюджет**: определение, что можно купить на заданную сумму.
- **Покупка товаров**: списание партии товаров из магазина.
- **Поиск магазина для минимальной стоимости партии товаров**.
- **Просмотр всех товаров** в системе.
- **Просмотр всех магазинов и их ассортимента.**

---

## 🛠 Используемый стек технологий

### Язык программирования
- **C#** – современный язык для написания высокопроизводительных приложений.

### Фреймворки и библиотеки
- **.NET 8** – платформа для разработки приложений.
- **Entity Framework Core (EF Core)** – ORM для взаимодействия с базой данных.
- **SQLite** – легковесная реляционная база данных.

### Архитектура
Проект организован по многослойной архитектуре:
1. **Presentation** – консольное приложение, точка входа. Здесь происходит взаимодействие с пользователем.
2. **BLL (Business Logic Layer)** – слой бизнес-логики, содержит основные функции приложения.
3. **DAL (Data Access Layer)** – слой доступа к данным, включает модели, контекст EF Core и репозитории.

---

## 🏗 Архитектура проекта

ShopSolution
├── ShopSolution.sln                // Файл решения  
├── ShopSolution.Presentation/      // Точка входа  
│   ├── Program.cs                  // Логика консольного приложения  
│   ├── appsettings.json            // Конфигурация (строка подключения)  
├── ShopSolution.BLL/               // Бизнес-логика  
│   ├── DTO/                        // Data Transfer Objects  
│   │   ├── ProductDTO.cs  
│   │   ├── StoreDTO.cs  
│   │   └── PurchaseItemDTO.cs  
│   ├── Services/                   // Сервисы  
│       ├── IShopService.cs  
│       └── ShopService.cs  
├── ShopSolution.DAL/               // Доступ к данным  
│   ├── Models/                     // Модели базы данных  
│   │   ├── Product.cs  
│   │   ├── Store.cs  
│   │   └── StoreProduct.cs  
│   ├── Context/                    // DbContext для EF Core  
│   │   ├── ShopContext.cs  
│   │   ├── DesignTimeDbContextFactory.cs // Для миграций  
│   ├── Repositories/               // Репозитории  
│       ├── IProductRepository.cs  
│       ├── IStoreRepository.cs  
│       ├── IStoreProductRepository.cs  
│       ├── RelationalProductRepository.cs  
│       ├── RelationalStoreRepository.cs  
│       └── RelationalStoreProductRepository.cs  
├── README.md                       // Документация проекта  


## ⚙ Установка и запуск

### Требования
- Установленный **.NET 8 SDK**
- Установленный **dotnet-ef** (глобальный инструмент для EF Core)