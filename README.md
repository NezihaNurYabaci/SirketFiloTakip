# Fleet Tracking System

Fleet Tracking System is a web-based application developed to manage company vehicles, employees, task assignments, and vehicle-related operational records through a centralized system.

The project was developed during my software internship to gain practical experience with backend development, database management, RESTful APIs, and frontend-backend integration.

## Features

- Vehicle and employee management
- Task creation and assignment
- Vehicle and employee availability checks
- Prevention of overlapping task assignments
- Driving license validation based on vehicle requirements
- Task start and completion operations
- Vehicle mileage tracking
- Fuel record management
- Maintenance record management
- Damage record management
- Fleet cost and usage reports
- Dashboard for viewing fleet information

## Technologies

### Backend
- C#
- ASP.NET Core Web API
- Entity Framework Core
- LINQ
- RESTful API

### Database
- Microsoft SQL Server
- Entity Framework Core Migrations

### Frontend
- HTML
- CSS
- JavaScript

### Tools
- Visual Studio
- SQL Server Management Studio
- Swagger / OpenAPI
- Git
- GitHub

## Project Structure

- **Controllers** – Handle API requests and endpoints.
- **Services** – Contain business logic for the main modules.
- **Models** – Represent the main entities of the system.
- **DTOs** – Manage data transferred between the API and client.
- **Data** – Contains the Entity Framework Core database context.
- **Migrations** – Contains database schema migrations.
- **wwwroot** – Contains the frontend files.

## Main Modules

The system consists of six main operational modules:

- Vehicle
- Employee
- Task
- Fuel Record
- Maintenance Record
- Damage Record

Vehicles and employees can be assigned to tasks, while fuel, maintenance, and damage records are associated with vehicles.

## Business Rules

Before a task is created, the system checks whether the selected vehicle and employee are available during the requested time period. Overlapping assignments are prevented, and the employee's driving license is validated according to the vehicle requirements.

During task execution, actual departure and return information is recorded together with starting and ending mileage. Vehicle status and mileage are updated according to task operations.

## Reporting

The reporting module provides information such as:

- Available vehicles
- Vehicles ordered by mileage
- Fuel cost per vehicle
- Maintenance cost per vehicle
- Total operational cost per vehicle
- Number of tasks completed by employees

## Running the Project

### Requirements

- .NET SDK
- SQL Server or SQL Server Express
- Visual Studio or another compatible IDE

### Setup

Clone the repository:

```bash
git clone https://github.com/NezihaNurYabaci/SirketFiloTakip.git
```

Configure the SQL Server connection string in `appsettings.json`.

Apply the Entity Framework Core migrations:

```bash
dotnet ef database update
```

Run the application:

```bash
dotnet run
```

The API endpoints can be explored and tested through Swagger while the application is running.

## About the Project

This project was developed as part of my software internship. It provided practical experience with C#, ASP.NET Core Web API, Entity Framework Core, SQL Server, CRUD operations, DTOs, service-based organization, validation, business rules, LINQ queries, REST APIs, and frontend-backend integration.