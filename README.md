# Blocked Countries and IP Validation API

## Overview

This .NET Core Web API allows managing blocked countries and validating IP addresses using third-party geolocation APIs (ipapi.co). The application uses in-memory storage instead of a database for storing blocked countries and logs.

## Features

- **Block a Country**: Add a country to the blocked list.
- **Unblock a Country**: Remove a country from the blocked list.
- **Retrieve Blocked Countries**: Get all blocked countries with pagination and filtering.
- **IP Lookup**: Retrieve country details from an IP address.
- **Check if IP is Blocked**: Automatically fetch the caller’s IP and check if it belongs to a blocked country.
- **Log Failed Access Attempts**: Store and retrieve attempts from blocked IPs.
- **Temporarily Block a Country**: Block a country for a specific duration with automatic expiration handled by a background service.

## Important Notes

- **Local Development Limitation:** If running the app locally, the IP fetched from `HttpContext` will be `::1` since the request comes from the same machine as the server. This means that the third-party API will return `null` for all properties as `::1` is not a valid external IP.
- **In-Memory Storage:** The application does not use a database; all data is stored in-memory.

## Setup Instructions

### 1. Clone Repository

```sh
git clone <repository_url>
cd <repository_folder>
```

### 2. Install Dependencies

```sh
dotnet restore
```

### 3. Configure Third-Party API Key

Sign up for a free API key from [ipapi.co](https://ipapi.co/) and update `appsettings.json`:

```json
{
  "IpGeolocation": {
    "ApiKey": "your_api_key_here",
  }
}
```

### 4. Run the API

```sh
dotnet run
```

The API will be available at `http://localhost:5000` (or another assigned port).

## API Endpoints

### 1. Block a Country

- **Endpoint:** `POST /api/countries/block`
- **Request Body:** `{ "countryCode": "US" }`
- **Action:** Adds the country to the in-memory blocked list.

### 2. Unblock a Country

- **Endpoint:** `DELETE /api/countries/block/{countryCode}`

### 3. Get Blocked Countries

- **Endpoint:** `GET /api/countries/blocked?page=1&pageSize=10&filter=US`

### 4. IP Lookup

- **Endpoint:** `GET /api/ip/lookup?ipAddress=8.8.8.8`
- **If omitted:** Uses caller’s IP.

### 5. Check if IP is Blocked

- **Endpoint:** `GET /api/ip/check-block`
- **Action:** Fetches caller's IP, retrieves country details, and checks if it’s blocked.
- **Note:** If running locally, the IP will be `::1` (localhost), and the third-party API will return `null` for all properties.

### 6. Retrieve Blocked Logs

- **Endpoint:** `GET /api/logs/blocked-attempts?page=1&pageSize=10`

### 7. Temporarily Block a Country

- **Endpoint:** `POST /api/countries/temporal-block`
- **Request Body:** `{ "countryCode": "US", "durationMinutes": 120 }`
- **Action:** Blocks the country for the given duration, automatically unblocking it afterward.

## Tools & Technologies

- **.NET Core 7/8/9**
- **HttpClient** (for API integration)
- **ConcurrentDictionary** (for thread-safe in-memory storage)
- **Swagger** (for API documentation)

## API Documentation

Swagger UI is available at:

```
http://localhost:5000/swagger
```

## License

This project is for evaluation purposes and does not include a specific license.

