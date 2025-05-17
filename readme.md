

#  EventsCandy

## ⚠️⚠️ the backend is working very well the front has some issues but also have working things like Events page (countians search and filter by category and price)

## 📂 Folder Structure

```
EventBookingSystem/
├── readme.md
├── src/
│   ├── EventBookingSystem.Api/
│   │   ├── Connected Services/
│   │   ├── Dependencies/
│   │   ├── Properties/
│   │   ├── Controllers/
│   │   ├── appsettings.json
│   │   ├── EventBookingSystem.Api.http
│   │   └── Program.cs
│   │
│   ├── EventBookingSystem.Core/
│   │   ├── Dependencies/
│   │   ├── DTOs/
│   │   │   ├── Bookings/
│   │   │   │   ├── BookingCreateDto.cs
│   │   │   │   └── BookingDto.cs
│   │   │   ├── Events/
│   │   │   │   ├── CreateEventDto.cs
│   │   │   │   ├── EventDto.cs
│   │   │   │   └── EventUpdateDto.cs
│   │   │   └── User/
│   │   │       ├── AuthResponseDto.cs
│   │   │       ├── DashboardOverviewModel.cs
│   │   │       ├── LoginReq.cs
│   │   │       ├── RefreshTokenReq.cs
│   │   │       ├── RegisterReq.cs
│   │   │       ├── UpdateUserReq.cs
│   │   │       └── UserDto.cs
│   │   └── Entities/
│   │
│   ├── EventBookingSystem.Infrastructure/
│   │   ├── Dependencies/
│   │   ├── Data/
│   │   │   ├── Config/
│   │   │   │   ├── ApplicationRoleConfiguration.cs
│   │   │   │   ├── ApplicationUserConfiguration.cs
│   │   │   │   ├── EventConfiguration.cs
│   │   │   │   └── RefreshTokenConfiguration.cs
│   │   │   └── ApplicationDbContext.cs
│   │   ├── Migrations/
│   │   │   ├── 20250517102800_asdfdasf.cs
│   │   │   ├── 20250517132547_asdfdasfaa.cs
│   │   │   └── ApplicationDbContextModelSnapshot.cs
│   │   └── Repositories/
│   │
│   ├── EventBookingSystem.Webpage/
│   │   ├── Connected Services/
│   │   ├── Dependencies/
│   │   ├── Properties/
│   │   ├── wwwroot/
│   │   │   ├── css/
│   │   │   │   ├── admin.css
│   │   │   │   ├── app.css
│   │   │   │   ├── auth.css
│   │   │   │   ├── loading.css
│   │   │   │   └── profile.css
│   │   │   ├── images/
│   │   │   ├── lib/
│   │   │   ├── appsettings.json
│   │   │   ├── favicon.png
│   │   │   ├── icon-192.png
│   │   │   └── index.html
│   │   └── Pages/
│   │       ├── Admin/
│   │       │   └── Components/
│   │       │       ├── BookingManagement.razor
│   │       │       └── DashboardOverview.razor
│   │
│   └── Shared/     # Common shared components across projects
│
├── Entities/       # Domain entity models
│   ├── ApplicationRole.cs
│   ├── ApplicationUser.cs
│   ├── Booking.cs
│   ├── Event.cs
│   ├── GlobalVariable.cs
│   └── RefreshToken.cs
│
├── Interfaces/     # Interfaces for repositories and services
│   ├── Repositories/
│   │   ├── IBookingRepository.cs
│   │   ├── IEventRepository.cs
│   │   └── IUserRepository.cs
│   └── Services/
│       ├── IAdminService.cs
│       ├── IAuthService.cs
│       ├── IBookingService.cs
│       ├── IEventService.cs
│       └── ITokenService.cs
│
├── Repositories/   # Implementation of repository interfaces
│   ├── BookingRepository.cs
│   ├── EventRepository.cs
│   └── UserRepository.cs
│
└── Services/       # Implementation of service interfaces
    ├── AdminService.cs
    ├── AuthenticationHeaderHandler.cs
    ├── AuthService.cs
    ├── BookingService.cs
    ├── ClientAuthService.cs
    ├── CustomAuthStateProvider.cs
    ├── EventService.cs
    ├── TokenService.cs
    └── UserService.cs
```

### Pages Structure
```
Pages/
├── Admin/
│   └── Components/
│       ├── BookingManagement.razor
│       ├── DashboardOverview.razor
│       ├── EventManagement.razor
│       └── UserManagement.razor
│   └── Dashboard.razor
├── Component.razor
├── EventDetails.razor
├── Events.razor
├── Index.razor
├── Login.razor
├── Profile.razor
└── Signup.razor
```

### Shared Components
```
Shared/
├── AdminLayout.razor
├── Footer.razor
├── Header.razor
├── MainLayout.razor
├── _Imports.razor
└── App.razor
```


---

## 🔗 Controllers & Endpoints

### 1. AuthController
- **POST** `/api/auth/register` - Register a new user
- **POST** `/api/auth/login` - Authenticate user and return token
- **GET** `/api/auth/me` - Get current authenticated user

### 2. EventController
- **GET** `/api/events` - Get all events (with optional filtering)
- **POST** `/api/events` - Create a new event (admin only)
- **GET** `/api/events/:id` - Get event details by ID
- **PUT** `/api/events/:id` - Update an event (admin only)
- **DELETE** `/api/events/:id` - Delete an event (admin only)
- **GET** `/api/events/categories` - Get all event categories

### 3. BookingController
- **GET** `/api/bookings` - Get all bookings for current user
- **POST** `/api/bookings` - Create a new booking
- **GET** `/api/bookings/:id` - Get booking details by ID
- **PUT** `/api/bookings/:id` - Update booking status
- **DELETE** `/api/bookings/:id` - Cancel booking
- **GET** `/api/bookings/events/:eventId` - Check if user has booked event

### 4. UserController
- **GET** `/api/users` - Get all users (admin only)
- **GET** `/api/users/:id` - Get user details by ID (admin or own profile)
- **PUT** `/api/users/:id` - Update user profile (admin or own profile)
- **DELETE** `/api/users/:id` - Delete user (admin only)

### 5. AdminController
- **GET** `/api/admin/dashboard` - Get admin dashboard statistics
- **GET** `/api/admin/bookings` - Get all bookings (admin only)
- **PUT** `/api/admin/bookings/:id` - Update any booking (admin only)


---


## 🎛️ User Roles & Access Control


### User not registered

![[Pasted image 20250517220107.png]]


### Registered User

![[Pasted image 20250517220125.png]]


### Admin 

![[Pasted image 20250517220147.png]]


---

## 🗺️ Page Structure map


![[Pasted image 20250517220256.png]]

![[Pasted image 20250517220310.png]]


---

## 📌 Build and run 

To build and run your Event Booking System project, follow these comprehensive steps:

First, make sure you have all the necessary prerequisites installed:

- .NET 9 SDK
- Visual Studio 2022 or VS Code with C# extensions
- SQL Server (Express or Developer edition)
- Git (if using version control)

The database setup is straightforward - create a new database named "EventBookingSystem" and update the connection string in the appropriate configuration files.

For applying migrations, use Entity Framework Core commands like:

```
dotnet ef database update
```

Building is simple with either Visual Studio (Ctrl+Shift+B) or via command line:

```
dotnet restore
dotnet build
```

When running the project, you'll need to launch both the API and the Blazor web application. In Visual Studio, you can configure multiple startup projects to run them simultaneously.

If you encounter issues, the guide includes troubleshooting tips for database connection problems, build errors, and runtime issues.

Finally, when you're ready to deploy, you can publish both the API and web application with simple dotnet publish commands.


---

## 🧷 ERD 

erDiagram
    ApplicationUser ||--o{ RefreshToken : "has"
    ApplicationUser ||--o{ Booking : "makes"
    Event ||--o{ Booking : "has"
    
    ApplicationUser {
        Guid Id PK
        string UserName
        string Email
        string PhoneNumber
        string PasswordHash
        string FirstName
        string LastName
        DateTime DateOfBirth
        DateTime CreatedAt
        DateTime UpdatedAt
        bool IsEmailVerified
        bool IsPhoneVerified
    }
    
    ApplicationRole {
        Guid Id PK
        string Name
        string Description
        DateTime CreatedAt
    }
    
    RefreshToken {
        Guid Id PK
        Guid UserId FK
        string Token
        string CreatedByIp
        string RevokedByIp
        string ReblacedToken
        DateTime CreatedAt
        DateTime RevokedAt
        DateTime ExpireDate
        bool IsExpired
        bool IsActive
    }
    
    Booking {
        Guid Id PK
        Guid UserId FK
        Guid EventId FK
        DateTime BookingDate
    }
    
    Event {
        Guid Id PK
        string Name
        string Description
        DateTime EventDate
        string Venue
        decimal Price
        string ImageUrl
        bool IsBooked
        DateTime CreatedAt
        EventCategory Category
    }
    
    EventCategory {
        int Value
        string Name
    }

---

# FrontEnd 


### Main page

![[Pasted image 20250517225301.png]]
![[Pasted image 20250517225412.png]]
![[Pasted image 20250517225429.png]]


### Events Page

![[Pasted image 20250517225519.png]]

### Signup and Login 

![[Pasted image 20250517225606.png]]
![[Pasted image 20250517225624.png]]


### Admin Dashboard 

![[Pasted image 20250517225720.png]]

![[Pasted image 20250517225742.png]]
![[Pasted image 20250517225754.png]]
![[Pasted image 20250517225807.png]]


----


##  🥲 I am sorry I didn't have enough time to make the project completely perfect it and put the final touches because of exams and competitions in college but the project is still very good and the Backend part is not 

