# 📚 TheBookClub

**TheBookClub** is an **ASP.NET Core Web API** designed to manage book-related operations, user interactions, and real-time notifications.
## 🚀 What is done inside?
- Role-based access control (`Admin`, `User`)
- JWT-based authentication for secure API access and role-based access control
- Two-factor authentication (2FA) for enhanced security
- **Rate Limiting** to avoid too many requests and Prevents Abuse and Overuse
- Real-time notifications via **SignalR**
- Role management and user account control
- **File** download support for book content
- Filter/sort by different attributes
- **soft delete functionality** for preserving data integrity
- CRUD operations

📚 Book Store Management
🔖 Bookmarking and Recommendations
🔔 Real-Time Notifications of Book arrival
🔍 Advanced Search and Filtering
📊 Admin tools to manage the store

## 🛠 Technologies Used

### ✅ Backend
- ASP.NET Core 8.0
- Entity Framework Core 9.0.3 (with SQL Server)
- SignalR for real-time communication
- AutoMapper for object mapping

### ✅ Authentication and Authorization
- ASP.NET Core Identity
- JWT Bearer Authentication

### ✅ Database
- Microsoft SQL Server

### ✅ Other Libraries
- Swashbuckle (Swagger) for API documentation
- Microsoft.Extensions.Logging for logging
- Newtonsoft.Json for JSON serialization
- Encodings.Web for QR code generation

---


## ⚙️ Setup and Configuration

### ✅ Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- SQL Server
- Visual Studio 2022 or VS Code

### 🧪 How to Fork and Run
```
1. Clone the repository
git clone https://github.com/belteshazzer/TheBookClub.git

2. Install dependencies by running the following
dotnet restore

3. create TheBookClub database on SQL
4. Configure database in appsettings.json
  "ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;User=qwert;Password=qwertyui;Database=TheBookClubDB;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true"
}
5. Apply migrations
dotnet ef database update

6. Update your Email Setting
"EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUser": "youremail@gmail.com",
    "SmtpPass": "email's password", // generate an app password if you're using 2FA for your email
    "EnableSsl": true,
    "SenderName": "The Book Club"
  },

6. Run the application
dotnet run
```
Access Swagger UI at:
👉 https://localhost:5001/swagger

### 🔑 Key API Endpoints
## 🔐 Authentication
`POST /api/auth/login` - Login and receive JWT token

`POST /api/auth/register` - Register a new user

## 📚 Books
`GET /api/books` - Get all books

`POST /api/books` - Add new book (Admin only)

`DELETE /api/books/{id}` - Soft delete (Admin only)

## 🔔 Notifications
`POST /api/notifications/SendNotification/{userId}` - Send to a user

`POST /api/notifications/SendGroupNotification` - Send to a group

## 👥 Roles
`GET /api/roles` - Get all roles (Admin only)

`POST /api/roles` - Add a new role (Admin only)

