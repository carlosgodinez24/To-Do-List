# To-Do List Application
The To-Do List Application is a full-stack project that allows users to manage their personal to-do lists through a web interface. The application consists of a .NET 7 RESTful API backend and an Angular frontend. Users can create, read, update, and delete tasks, with authentication ensuring that each user can access only their own tasks.

![image](https://github.com/user-attachments/assets/4d20c0ba-5c7a-454d-a5ee-5f1db52996b2)

![image](https://github.com/user-attachments/assets/52408b12-5596-4816-82a2-d56c4e78e4d9)

## Features

- **User Authentication:** Secure login using JWT tokens.
- **Task Management:** Perform CRUD operations on tasks.
- **Authorization:** Ensure users can only access their own tasks.
- **Responsive UI:** An intuitive Angular interface that works across devices.
- **Code-First Migration:** Database schema is created from code models.
- **Unit Testing:** Test coverage for controllers and services.

## Tech Stack
- **Back-End:**
  - **Language:** C#
  - **Framework:** .NET 7
  - **Database:** SQL Server
  - **ORM:** Entity Framework Core 7
  - **Authentication:** JWT Bearer Tokens
  - **Testing:** xUnit, Moq
  - **IDE:** Visual Studio 2022 or later
- **Front-End:**
  - **Framework:** Angular v16
  - **Language:** TypeScript
  - **UI Library:** Angular Material

## Prerequisites
- **For the API:**
  - .NET 7 SDK: [Download](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
  - SQL Server: [Download (Express or Developer edition)](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  - Visual Studio 2022: [Download](https://visualstudio.microsoft.com/downloads/)
  - Git: [Download](https://git-scm.com/downloads)
  - Postman or similar tool for testing API endpoints (optional)
- **For the Angular App:**
  - Node.js (version 14 or higher)
  - npm (comes with Node.js)
  - Angular CLI v16: : Install globally using `npm install -g @angular/cli@16`

## Getting Started

### Clone the Repository
```bash
git clone https://github.com/carlosgodinez24/To-Do-List.git
```
### API Configuration
1. Update Connection String

In `appsettings.json`, update the `DefaultConnection` string to match your SQL Server instance:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ToDoListDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
For **SQL Server Express**, use:
```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ToDoListDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

2. JWT Settings

The JWT settings are pre-configured in `appsettings.json`:
```json
"Jwt": {
  "Key": "YourVerySecureSecretKey12345",
  "Issuer": "ToDoListAPI",
  "Audience": "ToDoListAPIUsers"
}
```
- **Note:** If you plan to deploy the API to production, replace the Key with a secure, randomly generated string and store it safely.

### Database Setup
The project includes the necessary migrations to set up the database schema, including a default user.

1. Restore NuGet Packages
In the Package Manager Console (Visual Studio):
```bash
Update-Package
```
2. Update the Database
Apply the existing migrations to create the database and schema:
```bash
Update-Database
```
- This command will create the ToDoListDB database and apply all migrations.
- **Note:** There's no need to create new migrations unless you've made changes to the models.

### Running the Application
1. Build the project to ensure all dependencies are resolved.

2. Run the API
- Press **F5** in Visual Studio to run the API.
- The API should now be running at http://localhost:3000 if configured.

To run the API on a specific port, modify launchSettings.json in the Properties folder:

```json
"applicationUrl": "http://localhost:3000"
```

## API Overview
### Login
- **Endpoint:** POST /api/User/login
- **Description:** Authenticates a user and returns a JWT token.
- **Default User Credentials:**
  - Username: `admin`
  - Password: `password1`
### Task Management
All task-related endpoints require the Authorization header with the JWT token:

- **GET /api/Task:** Retrieves all tasks for the authenticated user.
- **GET /api/Task/{id}:** Retrieves a specific task by TaskId.
- **POST /api/Task:** Creates a new task for the authenticated user.
- **PUT /api/Task/{id}:** Updates an existing task.
- **PATCH /api/Task/{id}/status:** Updates the completion status of a task.
- **DELETE /api/Task/{id}:** Deletes the specified task.

# Contact
For any questions or suggestions, please contact cgodinez24001@gmail.com.
