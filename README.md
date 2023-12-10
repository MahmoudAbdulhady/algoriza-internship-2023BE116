DataBase Link: [https://drive.google.com/file/d/1aKZK8b-43ZO5uuMAqxLJ1nE7Bzw-wUcX/view?usp=sharing](https://drive.google.com/file/d/1sQwduU3XC1MOwo86YNwqp2scushL_-uV/view?usp=sharing)

# Introduction
Doctor/Patient Booking System Mock With Asp.net Web api EndPoints 
You can use the Endpoints to add doctors , specalizations , doctor Appointments , Create Patients , Book Appointments , Cancel Appointments 



###  :electric_plug: Installation

Prerequisites
.NET 6 SDK
Visual Studio 2022 or later (or suitable editor like VS Code)
SQL Server (or any other EF Core compatible database)


Detailed Installation Steps

1-Clone the Repository
[https://github.com/your-repository/Veezta.git](https://github.com/MahmoudAbdulhady/algoriza-internship-2023BE116)
2- Database Configuration
Update the appsettings.json file in the project root with your database connection string. This step is crucial for connecting the API to your SQL Server database.

3-Apply Database Migrations (Incase you  don't want to use the backup database you can use the following)
Use Entity Framework Core to set up your database schema. Run the following command:
add-migration 
update-database

4-Make the veezta as the startup project and run the application


## :zap: Features
Doctor Management
Add Doctors: Easily onboard new doctors into the system with detailed profiles.
Remove Doctors: Safely remove doctors from the system when they are no longer available or affiliated.
Sending The Registered Email and Password 

Patient Management
Patient Registration: Quick and easy registration process for new patients, creating comprehensive patient profiles.
Appointment Booking: Patients can book appointments with their preferred doctors, selecting available time slots based on their convenience.
Appointment Cancellation: Provides flexibility for patients to cancel their appointments, with notifications sent to the respective doctor.

Appointment Management
Create Appointments: Admins can create appointments for patients, assigning them to the appropriate doctors.
Manage Appointments: Comprehensive management of appointment schedules, including rescheduling and reminders.

Vouchers and Discounts
Voucher System: Implement a voucher system where patients can redeem vouchers for discounts or special offers on medical services based on the number of completed requests.




###  :file_folder: File Structure
```
Veezta
├── Application
│   ├── Contracts
│   ├── DTOs
│   └── Services
├── Domain
│   ├── DTOs
│   ├── Entities
│   ├── Enums
│   └── Interfaces
├── Infrastructure
│   ├── Data
│   ├── Migrations
│   └── Repositories
├──  Veezta
|    ├── Controllers
|    ├── wwwroot
|    ├── appsettings.json
|    └── Program.cs





