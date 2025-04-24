# Examy

# Examy - Online Quiz Platform

Examy is a modern web-based quiz platform built with Blazor WebAssembly and .NET 9. It allows administrators to create and manage quizzes while students can take quizzes and view their results.

## Features

- User authentication and authorization (Admin/Student roles)
- Quiz creation and management
- Category-based 
- Quiz results 

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 or later / Visual Studio Code
- SQL Server (Local or Express)

## Project Structure

- **Examy.Api**: Backend API project
- **Examy.Web**: Blazor WebAssembly frontend project
- **Examy.Shared**: Shared models and interfaces
- **Examy.Shared.Components**: Reusable Blazor components

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/Examy.git
cd Examy
```

### 2. Configure the Database

1. Open `Examy.Api/appsettings.json`
2. Update the `DefaultConnection` string to point to your SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ExamyDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Apply Database Migrations

```bash
cd Examy.Api
dotnet ef database update
```

## Running the Application

### Option 1: Using Visual Studio

1. Open `Examy.Web.sln` in Visual Studio
2. Right-click on the solution in Solution Explorer
3. Select "Configure Startup Projects..."
4. Choose "Multiple startup projects"
5. Set both `Examy.Api` and `Examy.Web` to "Start"
6. Click "Apply" and "OK"
7. Press F5 or click the "Start" button

### Option 2: Using Command Line

1. Start the API (in one terminal):
```bash
cd Examy.Api
dotnet run
```

2. Start the Blazor WebAssembly app (in another terminal):
```bash
cd Examy.Web
dotnet run
```

The application will be available at:
- API: https://localhost:7000
- Web App: https://localhost:7001

## Default Credentials

Admin account:
- Email: admin@gmail.com
- Password: Admin@123
  - Email: student@gmail.com
- Password: Student@123


# Student portal 
![image](https://github.com/user-attachments/assets/672f41c9-ee45-4c61-84fa-56670913eb3b)

![image](https://github.com/user-attachments/assets/fe3fa885-7c41-4547-8303-db3f2c033072)

![image](https://github.com/user-attachments/assets/ba804cfd-c3f1-4fc2-a80c-3acfd8f74d53)
![image](https://github.com/user-attachments/assets/dd4f6835-563d-4fc1-abf4-8cad67a13aed)

# Admin portal
![image](https://github.com/user-attachments/assets/628d03aa-e20c-436d-a429-cebf119583dd)
![image](https://github.com/user-attachments/assets/3fc28961-77f7-4905-b5af-2027e5ef286d)
![image](https://github.com/user-attachments/assets/3bb8c4ab-e74a-4be7-a33e-f47c9d240273)
![image](https://github.com/user-attachments/assets/74250dc1-0f70-4ee8-9070-e8a6919a3aa1)
![image](https://github.com/user-attachments/assets/621407af-0344-444f-812a-b015526a2325)
![image](https://github.com/user-attachments/assets/3cf84661-6048-44f1-b8e0-312f5fa71c58)
![image](https://github.com/user-attachments/assets/0b481ff2-1c36-4f56-a1e1-9cd03a332b44)






