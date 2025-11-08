# KeylineNet

A .NET client library for interacting with the Keyline API.

## Features

- **Simple API Client**: Easy-to-use client for interacting with Keyline projects and applications
- **Authentication Support**: Built-in support for service user authentication with JWT tokens
- **Type-Safe**: Strongly typed API with interfaces for better IntelliSense support
- **Modern .NET**: Built on .NET 9.0

## Installation

```bash
dotnet add package KeylineClient --version 0.0.1
```

## Usage

### Basic Example

```csharp
using KeylineClient;
using KeylineClient.auth;

// Create Keyline client
var keylineClient = new ClientFactory("http://localhost:8081", "keyline")
    .WithServiceUserAuth(
        "admin-ui",
        "test-service-user", 
        PrivateKeyProviderFactory.Static(PrivateKeyPem, "9ca0ce76-7f8d-4675-8af7-3b34b04ac976")
    ).Create();


// Use the client to interact with Keyline
var applications = await keylineClient.Projects["system"].Applications.List();
applications.Items.ForEach(x => Console.WriteLine(x.Name));
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
