# URL Shortener API Documentation

## Endpoints

### Shorten URL

`POST /shorten`

Creates a shortened URL from a long URL.

#### Request

-   Method: POST
-   Content Type: application/json
-   Parameter: `url` (string) - The URL to be shortened

#### Response

```json
{
	"originalUrl": "https://example.com/very/long/url",
	"shortCode": "abc123"
}
```

#### Status Codes

-   200: Success
-   400: Bad Request (empty URL)

### Redirect to Original URL

`GET /{shortCode}`

Redirects to the original URL associated with the provided short code.

#### Request

-   Method: GET
-   Parameter: `shortCode` (string) - The short code of the URL

#### Response

-   Redirects to the original URL

#### Status Codes

-   302: Found (redirect)
-   404: Not Found (invalid short code)

## Database Schema

### Url Table

-   Id (int, primary key)
-   OriginalUrl (string, required)
-   ShortCode (string, required, unique)
-   CreatedAt (DateTime)
-   LastAccessed (DateTime, nullable)
