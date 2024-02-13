Implementation of a web service. 

The service:
- accept an HTML file from a web client;
- convert it to PDF using Puppeteer Sharp,
- and return it somehow to the client.

Used:
- ASP.NET Core 6 Web API
- N-layered architecture
- Entity Framework Core
- SQLite as a database
- Event-sourcing in the context of the same service
- RabbitMQ as a message broker
- Puppeteer Sharp for generator
