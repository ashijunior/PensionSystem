# Pension Contribution Management System

## Overview
The **Pension Contribution Management System** is a robust application built with **.NET Core 8**, **Entity Framework Core** and **SQL Server**. 
It provides features for managing members, processing contributions, running background jobs, and enforcing data validation rules. 
This system is designed to handle pension-related operations efficiently and securely.

---

## Features

###	Member Management:
- Register, update, retrieve, and soft-delete members.

###	Contribution Processing:
-	Handle monthly and voluntary contributions.
-	Calculate total contributions and generate statements.
-	Enforce business rules (e.g., minimum contribution period for benefit eligibility).

###	Background Jobs:
-	Use Hangfire for:
  -	Validating contributions.
  -	Updating benefit eligibility.
  -	Calculating interest.
  -	Handling failed transactions and sending notifications.

### Data Validation:
-	Validate member details (name, date of birth, email, phone, age restrictions: 18-70).
-	Validate contributions (amount > 0, valid contribution date).
-	Validate employer registration (company name, valid registration, active status).

---

## Prerequisites
Before running the application, ensure the following are installed:
-	**.NET 8 SDK**
-	**SQL Server**
-	**Visual Studio 2022 (or any compatible IDE)**

---

## Setup Instructions
1.	Clone the Repository
2.	Update Database Connection String:
    -	Open the appsettings.json file and update the connection strings
3.	Run Database Migrations:
    -	Navigate to the project folder
4.	Run the Application:
    -	Start/Run the application

---

## API Documentation
The API documentation is available via Swagger UI.
### Key Endpoints
### Member Management
  -	Register Member: POST /api/member/register
  - Update Member: PUT /api/member/update/{memberId}
  -	Get Member: GET /api/member/{memberId}
  - Soft-Delete Member: DELETE /api/member/soft-delete/{memberId}
### Contribution Processing
  -	Add Monthly Contribution: POST /api/contribution/monthly
  -	Add Voluntary Contribution: POST /api/contribution/voluntary
  -	Get Total Contributions: GET /api/contribution/total/{memberId}
  -	Generate Statement: GET /api/contribution/statement/{memberId}
###Employer Management
  -	Register Employer: POST /api/employer/register

---

## Design Decisions
### Clean Architecture
The project follows Clean Architecture principles, separating the application into distinct layers:
  -	Domain: Contains business logic and entities.
  -	Application: Implements use cases and interfaces.
  -	Infrastructure: Handles data access and external services.
  -	Presentation: Includes the API controllers.
### Domain-Driven Design (DDD)
The system is designed around the core domain of pension contributions, with entities such as:
  -	Member
  -	Contribution
  -	Employer
### Repository and Unit of Work Patterns
These patterns were implemented to:
  -	Abstract data access logic.
  -	Ensure testability and maintainability.
### Hangfire for Background Jobs
Hangfire was chosen for its simplicity and reliability in handling background tasks, including:
  -	Contribution validation.
  -	Benefit eligibility updates.
  -	Interest calculations.
  -	Failed transaction handling and notifications.
