# EventService API

A Demo Event API built with .NET Core, leveraging essential technologies such as Entity Framework Core, Redis, MediatR, and XUnit. The API implements CRUD operations following clean architecture principles, applying CQRS with MediatR for command and query separation. It includes extensive integration and unit tests.

## Outline

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Testing](#testing)

## Prerequisites

Before running the Event API, ensure that you have the following prerequisites installed:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Redis](https://redis.io/docs/getting-started/)
- [Docker](https://www.docker.com/get-started) (optional)



## Installation

1. Clone the repository:

    ```
    git clone https://github.com/AdabanyaChinedu/EventService.git
    ```
    
2. Navigate to the project directory:

    ```
    cd EventService
    ```  
    
3. Build the project using the following command:

   ```
    dotnet build
    ```  
    
    
## Usage

1. Run the application using the following command:

    ```
    dotnet run
    ``` 
    
    The API will start running at `http://localhost:5000`.
    
2. Access the API endpoints using a tool like [Postman](https://www.postman.com/) or a web browser. Refer to the [API Endpoints](#api-endpoints) section for more details.




## API Endpoints

   The Event API provides the following endpoints:

    - `GET /api/v1/events` - Get all events (paginated).
    - `GET /api/v1/events/{id}` - Get an event by ID.
    - `POST /api/v1/events` - Create a new event.
    - `PUT /api/v1/events/{id}` - Update an existing event.
    - `DELETE /api/v1/events/{id}` - Delete an event.

   For detailed information on the request and response formats, please refer to the API documentation available at `http://localhost:5000/swagger` when the   
    application is running.



## Testing

The project includes extensive integration and unit tests to ensure the correctness of the implementation. To run the tests, follow these steps:

1. Navigate to the project directory (if you're not already there):

    ```
    cd EventService
    ``` 
    
2. Run the tests using the following command:

   ```
    dotnet test
    ``` 
    
  This command will execute the unit tests and display the test results in the console.
    

