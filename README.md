# KeylineNet

A .NET client library for interacting with the Keyline API.

## Features

- **Simple API Client**: Easy-to-use client for interacting with Keyline projects and applications
- **Authentication Support**: Built-in support for service user authentication with JWT tokens
- **Type-Safe**: Strongly typed API with interfaces for better IntelliSense support
- **Modern .NET**: Built on .NET 9.0

## Installation

Clone the repository and build the solution:

```bash
git clone https://github.com/The127/KeylineNet.git
cd KeylineNet
dotnet build
```

## Usage

### Basic Example

```csharp
using KeylineClient.auth;

// Create an authenticator
var authenticator = new ServiceUserAuthenticator(
    "http://localhost:8081/oidc/keyline/token",
    privateKeyPem,
    "test-service-user",
    "service-user-id",
    "client-id"
);

// Create an authentication handler
var authHandler = new AuthenticationHandler(authenticator)
{
    InnerHandler = new HttpClientHandler(),
};

// Create HTTP client
var httpClient = new HttpClient(authHandler);
httpClient.BaseAddress = new Uri("http://localhost:8081");

// Create Keyline client
var client = new KeylineClient.KeylineClient(httpClient, "keyline");

// Use the client to interact with Keyline
var pagedResponse = await client.Projects["system"].Applications.List();
pagedResponse.Items.ForEach(x => Console.WriteLine(x.Name));
```

## Development

### Prerequisites

- .NET 9.0 SDK or later
- Docker (optional, for running the PostgreSQL database)

### Running Tests

```bash
dotnet test
```

### Running the Example Application

The repository includes an example application demonstrating basic usage:

```bash
cd ExampleApp
dotnet run
```

### Database Setup

The project includes a Docker Compose file for running a PostgreSQL database:

```bash
docker-compose up -d
```

## Project Structure

- **KeylineClient**: The main client library
- **ExampleApp**: Example application demonstrating usage
- **UnitTests**: Unit tests for the client library
- **IntegrationTests**: Integration tests

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
