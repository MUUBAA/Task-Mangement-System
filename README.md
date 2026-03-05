# TaskManagement – ASP.NET Core Web API

A simplified **Task Management System** built as a Web API for a technical assessment.
The API uses **JWT authentication** with **two hardcoded users** (Admin + User) and enforces **role-based authorization**.

---

## What the API does

### Task properties

Each Task includes:

* `Id` (int)
* `Title` (string, required)
* `Description` (string, optional)
* `IsCompleted` (bool)
* `CreatedAt` (DateTime, auto-set on create)
* `DueDate` (DateTime?, optional)
* `OwnerUserId` (string, taken from JWT claim `NameIdentifier`)

### Functional rules

* Any authenticated user can **create** tasks.
* Users can **update only their own** tasks.
* Users can **view only their own** tasks.
* Admins can **view all** tasks.
* Only Admins can **mark tasks as completed**.

---

## Current Project Structure

```text
TaskManagement/
  Controllers/
    AuthController.cs
    TaskController.cs

  Data/
    Dto/
      LoginRequestDto.cs
      LoginResponseDto.cs
      TaskResponseDto.cs
      (TaskCreate/TaskUpdate DTOs)
    Entities/
    Exceptions/
    Repositories/
      TaskRepository.cs

  Migrations/
  Service/
    AuthService/
      AuthProvider.cs
      JwtTokenService.cs
      StaticUsers.cs
    TaskServices/
      TaskService.cs

  Utils/
  Program.cs
  appsettings.json
  appsettings.Development.json
  taskmanagement.db
  TaskManagement.http
```

---

## Requirements

* .NET SDK (same major version used by the project)
* SQLite (bundled via EF Core; no separate install required)

---

## Setup & Run

### 1) Configure JWT settings

Update `appsettings.json` (or `appsettings.Development.json`) with a **long secret** (important: HS256 needs **32+ bytes**):

```json
{
  "Jwt": {
    "Secret": "CHANGE_THIS_TO_A_LONG_SECRET_AT_LEAST_32_CHARS_123456",
    "Issuer": "TaskManagementApi",
    "Audience": "TaskManagementApi",
    "ExpiryMinutes": "60"
  }
}
```

### 2) Run the API

```bash
dotnet run
```

### 3) Swagger

Open Swagger in your browser:

* `https://localhost:<port>/swagger`

---

## Hardcoded Users (Login Credentials)

| Role  | Username | Password  |
| ----- | -------- | --------- |
| Admin | admin    | Admin@123 |
| User  | user     | User@123  |

These users are defined in:
`Service/AuthService/StaticUsers.cs`

---

## Authentication Flow (How to call protected endpoints)

### Step 1 — Login to get a JWT

**POST** `/api/auth/login`

Request:

```json
{
  "userName": "admin",
  "password": "Admin@123"
}
```

Response:

```json
{
  "accessToken": "<JWT_TOKEN>",
  "userId": "admin-1",
  "userName": "admin",
  "role": "Admin"
}
```

### Step 2 — Use the token

For all protected endpoints, add header:

```text
Authorization: Bearer <JWT_TOKEN>
```

In Swagger:

* Click **Authorize**
* Paste either:

  * `Bearer <token>`
    or
  * just `<token>`
    (Depends on your Swagger auth config)

---

## API Endpoints

Base URLs:

* Auth: `/api/auth`
* Tasks: `/api/tasks`

### Auth

#### ✅ Login

* **POST** `/api/auth/login`
* Public endpoint
* Returns JWT token + user details

(Optional if implemented)

#### Me (debug)

* **GET** `/api/auth/me`
* Returns current logged-in user claims

---

### Tasks (All require JWT)

#### ✅ Create Task

* **POST** `/api/tasks`
* Body:

```json
{
  "title": "Finish assessment",
  "description": "Clean code + JWT + RBAC",
  "dueDate": "2026-03-10T00:00:00Z"
}
```

Rules:

* `CreatedAt` auto-set
* `IsCompleted` defaults to `false`
* `OwnerUserId` from JWT claim `NameIdentifier`

---

#### ✅ View Tasks

* **GET** `/api/tasks`
  Behavior:
* User role: returns only own tasks
* Admin role: returns all tasks

---

#### ✅ Update Task

* **PUT** `/api/tasks/{id}`
* Body:

```json
{
  "title": "Updated title",
  "description": "Updated description",
  "dueDate": "2026-03-12T00:00:00Z"
}
```

Permissions:

* User can update only tasks they own
* Admin can update any task

---

#### ✅ Mark Task as Completed (Admin only)

* **PATCH** `/api/tasks/{id}/complete`
  Permissions:
* Admin only

Effect:

* Sets `IsCompleted = true`

---

## Database

The project uses EF Core with SQLite (based on presence of `taskmanagement.db` and migrations).

On startup, migrations are applied via:

* `db.Database.Migrate()`

So the DB is created/updated automatically.

---

## Common Troubleshooting

### HS256 key size error

Error:
`IDX10720 ... key size must be greater than 256 bits`

Fix:

* Make `Jwt:Secret` **32+ characters** (ASCII) in your active config file.

### 401 invalid_token: "The signature key was not found"

Usually means:

* You changed `Jwt:Secret` but are still using an older token
* You updated the wrong config source (Development file / user-secrets override)

Fix:

* Restart API
* Login again and use the new token
* Ensure `Jwt:Secret` is consistent in the active configuration

---

## Notes

* This project focuses on clean service/repository separation, DTO usage, and role-based authorization.

