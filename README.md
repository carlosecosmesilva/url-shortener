# URL Shortener

A simple and efficient URL shortener service built with .NET 6.

## Project Structure

```
├── src/                        # Source code
│   ├── Controllers/           # API Controllers
│   ├── Data/                  # Database context and configurations
│   ├── Models/               # Domain models and DTOs
│   │   ├── Url.cs
│   │   └── UrlDto.cs
│   ├── Services/             # Business logic services
│   │   ├── Interfaces/      # Service interfaces
│   │   └── UrlService.cs
│   └── Utils/               # Utility classes and helpers
├── tests/                     # Test projects
├── docs/                      # Documentation
└── README.md                 # Project documentation
```

## Prerequisites

-   .NET 6.0 SDK or later
-   SQLite

## Getting Started

1. Clone the repository

```bash
git clone https://github.com/carlosecosmesilva/url-shortener.git
cd url-shortener
```

2. Restore dependencies

```bash
dotnet restore
```

3. Run the application

```bash
dotnet run
```

The application will start at `https://localhost:5001` (or `http://localhost:5000`).

## API Endpoints

-   `POST /shorten` - Create a shortened URL
-   `GET /{shortCode}` - Redirect to the original URL

## Database

The application uses SQLite as its database. The connection string can be configured in `appsettings.json`.

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
