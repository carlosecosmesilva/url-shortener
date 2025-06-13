# 🔗 URL Shortener

This project is a URL shortening application composed of:

-   **Frontend**: React + TypeScript
-   **Backend**: ASP.NET Core (.NET 8) with Entity Framework Core and MySQL
-   **Database**: MySQL
-   **CI/CD**: GitHub Actions for client and server

---

## 🗂 Project Structure

```
url-shortener/
├── LICENSE
├── README.md
├── .github/
│   └── workflows/
│       ├── backend.yml            # CI for backend (.NET)
│       └── frontend.yml           # CI for frontend (React)
├── client/                        # React application
│   ├── public/                    # Static files
│   ├── src/
│   │   ├── api/                   # Axios config and API calls
│   │   ├── components/            # Reusable components
│   │   ├── pages/                 # Main pages
│   │   ├── hooks/                 # Custom React hooks
│   │   ├── types/                 # TypeScript types
│   │   ├── App.tsx
│   │   └── main.tsx
│   ├── tsconfig.json
│   ├── package.json
│   └── .env                      # Environment variables (e.g., VITE_API_URL)
├── server/
│   ├── .gitignore
│   └── UrlShortener.API/
│       ├── UrlShortener.API.sln
│       └── UrlShortener.API/
│           ├── Program.cs                # Application entry point
│           ├── appsettings.json
│           ├── appsettings.Development.json
│           ├── Controllers/             # REST endpoint controllers
│           ├── Models/                  # Domain models (ShortUrl, etc.)
│           ├── Data/                    # DbContext and EF configurations
│           ├── Repositories/            # Data access and abstractions
│           ├── Services/                # Business logic layer
│           ├── Migrations/              # EF Core migrations
│           ├── Seed/                    # Database seeding scripts
│           ├── Configurations/          # AutoMapper, CORS, etc.
│           └── Scripts/
│               └── init_database.sh     # CI/CD script to prepare the database

```

---

## 🚀 Features

-   Create new short URLs
-   Retrieve original URLs from short codes
-   Update an already shortened URL
-   Delete short URLs
-   Get statistics (number of accesses per link)

---

## 🔧 Technologies Used

### Frontend

-   [React](https://reactjs.org/)
-   [TypeScript](https://www.typescriptlang.org/)
-   [Axios](https://axios-http.com/)

### Backend

-   [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
-   [Entity Framework Core](https://learn.microsoft.com/ef/)
-   [Pomelo MySQL Provider](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql)

### Database

-   [MySQL 8+](https://www.mysql.com/)

### DevOps

-   [GitHub Actions](https://github.com/features/actions)

---

## 📦 Project Setup

### Prerequisites

-   Node.js
-   .NET 8 SDK
-   MySQL Server
-   Docker (optional, to run the database)

### Instructions

#### Backend

```bash
cd server/UrlShortener.API
dotnet restore
dotnet ef database update
dotnet run
```

#### Frontend

```bash
cd client
npm install
npm run dev
```

---

## 🧪 Testing

Both frontend and backend can be tested with:

-   Jest + React Testing Library (frontend)
-   xUnit or NUnit (backend)

---

## 📜 License

This project is licensed under the terms of the [MIT License](LICENSE).

---

## ✨ Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.
