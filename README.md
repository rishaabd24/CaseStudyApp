# CaseStudy

## Technologies

ASP.NET Core 5 with C#, Microsoft SQL Server as database, Authorization using Razor Pages and ASP Identity, and File Management System using Model-View-Controller code first approach.


## Features
- Registration and Login
- Upload files (Restricted to only .PBIX Files)
- Manage user details
- View File Metadata and Download

## To run on your machine

1. Clone the repository on your local machine
2. Open the Project file(CaseStudyApp.sln) in Microsoft Visual Studio
3. Navigate to "appsettings.json" and enter the name of your SQL Server under the "AuthConnectionString": "Server= _your server name_;..."
4. Go to Tools -> NuGet Package Manager -> Package Manager Console
5. In the Package Manager Console, run the command "Add-Migration YourMigrationName"
6. After Step 5, run the command "Update-Database" in the Package Manager Console and wait for "Done." confirmation.
7. Click on the green Run Project button.

## Screenshots

### Sign In Page
![Login](https://user-images.githubusercontent.com/63904466/177482580-f9be5aa1-eb94-43e2-8f04-6b926ef95a7d.png)

### Sign Up page
![image](https://user-images.githubusercontent.com/63904466/177527462-1fa6c4b5-ac87-427a-b136-ca15fcae5310.png)

### Home Page
![home](https://user-images.githubusercontent.com/63904466/177482720-30b58165-7f9b-4ef1-9ec5-454b693ff47b.png)

### Upload File Page
![upload](https://user-images.githubusercontent.com/63904466/177482778-7b880d38-9baf-484b-8b7e-1509d7e1cc47.png)

### User Management Page
![usermgmt](https://user-images.githubusercontent.com/63904466/177482829-6fe07fc7-0eb0-4b7d-bf90-c0cf519ad14d.png)

### Change Password Page
![pwd](https://user-images.githubusercontent.com/63904466/177482876-31a39dd5-e3aa-42c3-afc9-ab4a0a6e03e3.png)

### Password Change Confirmation Page
![pwdcnfrm](https://user-images.githubusercontent.com/63904466/177482939-85a46ad4-8c35-434b-87b3-1436424eb882.png)

