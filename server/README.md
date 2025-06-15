# 🚀 UrlShortener.API (Server)

This is the backend API for the URL Shortener project, built with ASP.NET Core (.NET 8), Entity Framework Core, MySQL, and MassTransit for messaging with RabbitMQ.

---

## 📋 What does this API do?

-   Provides a RESTful API for creating, updating, deleting, and retrieving short URLs.
-   Handles user authentication with JWT and refresh tokens.
-   Publishes and consumes events via RabbitMQ for key actions (such as URL creation, deletion, and failures).
-   Persists data in a MySQL database.

---

## 🛠️ Main Features

-   **Short URL Management:**  
    Create, update, delete, and retrieve short URLs. Each short URL is mapped to an original URL and tracks access statistics.

    > **Note:** The API generates a unique short code for each URL and exposes a complete short URL (e.g., `https://localhost:7115/r/abc12345`) based on the configured base URL.

-   **Authentication:**  
    Secure endpoints using JWT authentication. Supports login, registration, and token refresh.

-   **Messaging (RabbitMQ + MassTransit):**
    -   Publishes events when a short URL is created, deleted, or when creation fails.
    -   Consumers listen to these events for further processing or integration with other services.
    -   Useful for decoupling, audit logging, analytics, or triggering external workflows.

---

## ⚙️ Project Structure

```
UrlShortener.API/
├── Controllers/         # API endpoints (Auth, ShortUrl)
├── Data/                # DbContext and migrations
├── DTOs/                # Data Transfer Objects (requests/responses)
├── Messaging/
│   ├── Consumers/       # MassTransit consumers for RabbitMQ
│   ├── Contracts/       # Messaging contracts/interfaces
│   └── Messages/        # Message/event definitions
├── Models/              # Entity models (User, ShortUrl, etc.)
├── Repositories/        # Data access abstractions
├── Services/            # Business logic (JWT, ShortUrl, etc.)
├── appsettings.json     # Configuration (DB, JWT, ShortUrl base URL, etc.)
├── Program.cs           # Application entry point and DI setup
└── ...
```

---

## 📨 Messaging with RabbitMQ

-   **MassTransit** is used as the abstraction layer for messaging.
-   **RabbitMQ** is the message broker.
-   Events published:
    -   `ShortUrlCreated`
    -   `ShortUrlDeleted`
    -   `ShortUrlCreationFailed`
-   Consumers are registered for each event type and can be extended for additional processing.

**Example:**  
When a new short URL is created, a `ShortUrlCreated` event is published to RabbitMQ. Any service listening to this event (via a consumer) can react, e.g., for logging, analytics, or notifications.

---

## 🚦 How to Run

### Prerequisites

-   .NET 8 SDK
-   MySQL Server
-   RabbitMQ (running locally or accessible remotely)

### Setup

1. **Configure the database, RabbitMQ, and Short URL base** in `appsettings.json` if needed:
    ```json
    "ShortUrl": {
      "BaseUrl": "https://localhost:7115/r/"
    }
    ```
2. **Apply migrations** (if not already applied):
    ```sh
    dotnet ef database update
    ```
3. **Run the API:**

    ```sh
    dotnet run
    ```

    The API will be available at `https://localhost:7115` and/or `http://localhost:5142` (see `launchSettings.json`).

4. **Swagger UI:**  
   Access API documentation at `/swagger` (e.g., [https://localhost:7115/swagger](https://localhost:7115/swagger)).

---

## 🔒 Authentication

-   Endpoints are protected with JWT.
-   Use `/api/Auth/register` to create a user, `/api/Auth/login` to obtain tokens, and `/api/Auth/refresh` to refresh tokens.
-   Include the JWT token in the `Authorization: Bearer <token>` header for protected endpoints.

---

## 🌐 Short URL Example

When you create a short URL, the API returns a response like:

```json
{
	"id": 1,
	"originalUrl": "https://www.example.com",
	"shortCode": "abc12345",
	"createdAt": "2025-06-15T12:00:00Z",
	"accessCount": 0,
	"shortUrl": "https://localhost:7115/r/abc12345"
}
```

-   The `shortUrl` field is dynamically generated using the base URL configured in `appsettings.json` and the unique code.

### Example Request

```http
POST /api/ShortUrl
Content-Type: application/json
Authorization: Bearer <token>

{
  "originalUrl": "https://www.example.com"
}
```

---

## 📨 Messaging Example

-   When a short URL is created, the API publishes a `ShortUrlCreated` event to RabbitMQ.
-   Consumers (in `Messaging/Consumers/`) process these events for further actions.

---

## ⚙️ Environment Variables

Make sure to configure the following in your `appsettings.json` or environment:

-   MySQL connection string
-   RabbitMQ connection string
-   JWT secret and settings

---

## 📜 License

This project is licensed under the terms of the [MIT License](../LICENSE)
