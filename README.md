# 🔗 URL Shortener

A full-stack URL shortening application with authentication, statistics, and messaging support.

---

## 📚 Overview

This project is composed of two main parts:

-   **Frontend:** React + TypeScript ([client/](./client/))  
    A modern, responsive web interface for users to shorten URLs, manage their links, and authenticate securely.

-   **Backend:** ASP.NET Core (.NET 8) ([server/](./server/))  
    A RESTful API with JWT authentication, MySQL persistence, and messaging integration using RabbitMQ and MassTransit.

---

## 🗂️ Project Structure

```
url-shortener/
├── client/    # React frontend (see client/README.md)
├── server/    # ASP.NET Core backend (see server/README.md)
├── .github/   # CI/CD workflows
├── LICENSE
└── README.md  # (this file)
```

---

## 🚀 Quick Start

### Prerequisites

-   Node.js (for the frontend)
-   .NET 8 SDK (for the backend)
-   MySQL Server
-   RabbitMQ (for messaging, optional for basic usage)
-   Docker (optional, for running MySQL/RabbitMQ easily)

---

### 1. Backend Setup

See [server/README.md](./server/README.md) for full details.

**Basic steps:**

```sh
cd server/UrlShortener.API
dotnet restore
dotnet ef database update
dotnet run
```

-   The API will be available at `https://localhost:7115` and/or `http://localhost:5142`.
-   Access Swagger UI at `/swagger` for API documentation.

---

### 2. Frontend Setup

See [client/README.md](./client/README.md) for full details.

**Basic steps:**

```sh
cd client
npm install
npm run dev
```

-   The app will be available at [http://localhost:5173](http://localhost:5173).

---

## ⚙️ Configuration

-   **API URL:**  
    Set the backend API URL in `client/.env` (e.g., `VITE_API_URL=https://localhost:7115/api`).

-   **Database & Messaging:**  
    Configure MySQL and RabbitMQ connection strings in `server/UrlShortener.API/appsettings.json`.

-   **Authentication:**  
    The backend uses JWT and refresh tokens. See the server README for authentication endpoints and usage.

---

## 📦 Documentation

-   **Frontend:**  
    See [client/README.md](./client/README.md) for features, structure, and customization.

-   **Backend:**  
    See [server/README.md](./server/README.md) for API endpoints, messaging, authentication, and environment variables.

---

## 📝 License

This project is licensed under the terms of the [MIT License](./LICENSE).

---

## 🤝 Contributing

Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.
