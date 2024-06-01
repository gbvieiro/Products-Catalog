# Products-Catalog

Welcome to the product catalog. This project was developed to demonstrate an example of Clean Architecture using .NET 8 and based on Domain Driven Design principles. Here, we will manage a book catalog, create orders, and control the stock of these items.

# Prerequisites

- Visual Studio 2022

# Executing

1. Open the .sln file 'src\Products.Catalog.Domain.sln' with Visual Studio 2022.
2. Set the project 'Product.Catalog.API' as the startup project and execute it using 'F5'.
3. Once the project is opened, you can simply press "F5" to execute.
4. A URI will be opened in your browser; please allow Visual Studio to perform the necessary actions.
5. Select a method to execute and expand the component for more information.
6. Click on 'Try it Out' and fill in the requested parameters.
7. Take a look at the results.

## common issues

- This project has a authentication control, so a login is required to execute methods. Use the "Authorize button to inform a token".

# How-to

Here I will share some steps to simulate this API basic operations:

## 1. Create user:

- Use the "api/User/Save" method to create a admin User: 
```javascript
{
    "email": "admin@gmail.com",
    "password": "MyPassword",
    "role": "admin"
}
```
- The new user id is returned.
- Copy it to a notepad, will be requested in the next steps.
- Use the "api/Users/Save" method again to create a client User: 
```javascript
{
    "email": "client@gmail.com",
    "password": "MyPassword",
    "role": "client"
}
```

## 2. Generate a token

- Using the "api/Authentication/GenerateToken" to Generate a token to the admin user:

```javascript
{
    "email": "admin@gmail.com",
    "password": "MyPassword"
}
```

A token will be returned, copy it to a notepad.

## 3. Login

- Using the "Authorize" button, inform the returned token using this format:

```javascript
Bearer <your-token-here>
```

- Remember to click on the "Authorize" button in the modal to confirm operation.
- Now you are authenticated!
- Close the modal.

## 4. Get current user information: 

- Confirm that you are logged in using the "api/Authentication/GetCurrentUserInfo" method to get your user information.

## 5. Create a book

- Use the "/api/Books/Save" method to create a book:

```javascript
{
    "price": 22.50,
    "title": "The Shining",
    "author": "Stephen King",
    "genre": 10
}
```

- The new book will be returned with the new ID. Copy this id for the next step.

## 6. Insert stock for the created book:

- Use the "/api/Stocks/book/{bookId}/AddBooks" method to insert some items to stocks.
- Set the book Id on the field and a quantity of items in the body:

```javascript
{
    "quantity": 10
}
```

## 7. Verify current stock:

- Use the "/api/Stocks/book/{bookId}" method to confirm that stock quantity is updated.
 
## 8. Create a new order:

- Use the "/api/Orders/Save" method to create a new order for your custumer user:

```javascript
{
    "customerId": "{your-client-user-id-here}",
    "creationData": "2024-06-01T04:28:55.493Z",
    "status": 0,
    "items": [
    {
        "bookId": "{your-book-id-here}",
        "quantity": 2,
        "amount": 0
    }],
    "totalAmount": 0
}
```

## 10. Confirm that the order items were reserved:

- Use the "/api/Stocks/book/{bookId}" method again and verify the stock quantity.

## 10. Logout

- Perform a logout clicking in the "Authorize" button again.
- When the modal is showed click on "Logoff".
- Close the modal.

## 11. Generate token to the client user:

- Using the "api/Authentication/GenerateToken" to Generate a token to the client user:

```javascript
{
    "email": "client@gmail.com",
    "password": "MyPassword"
}
```

A token will be returned, copy it to a notepad.

## 12. Login with client user:

- Using the "Authorize" button, inform the returned token using this format:

```javascript
Bearer <your-token-here>
```

- Remember to click on the "Authorize" button in the modal to confirm operation.
- Now you are authenticated!
- Close the modal.

## 13. See client orders:

- Use the "/api/Orders/MyOrders" method to see current client user orders.
- This is a paginated API so we can set parameters with these values:

Text: "" // No filter text
skip: 0 // first page
Take: 10 // first 10 items

- The created order must be returned.
- Copy the order Id.

## 14. See the order details:

- Use the "/api/Orders/{id}" method to see register order details, informing the order id.

## 15. Cancel order

- Login with the Admin user again.
- cancel the order using "/api/Orders/{orderId}/cancel" method, informing the order id.
- A message informing that the order was canceled will be presented.

## 16. Verify stock again:

- Use the "/api/Stocks/book/{bookId}" method to confirm that stock quantity is updated.

# Unit tests

This solution contains some unit tests, follow above steps for running it and see some of the defined rules being validated:

- Open the Solution: Open Visual Studio 2022 and load the solution containing the test projects you want to run.

- Access the 'Test Explorer' window: Go to the top menu and select "Test" > "Test Explorer" or use the keyboard shortcut Ctrl + E, T. This will open the Test Explorer window, where you will see all available tests in the solution.

- Refresh the test list (optional): If the tests don't appear immediately in the Test Explorer window, click the "Refresh" button (circular arrow icon) to update the test list.

- Select the tests: In the Test Explorer window, you'll see a list of available tests. You can select all tests by right-clicking anywhere in the list and selecting "Run All Tests" from the context menu.

 - Run the tests: After selecting the tests you want to run, click the "Run All Tests" button at the top of the Test Explorer window. This will start the execution of all selected tests.

- Check the results: After the tests finish running, you'll see the results in the Test Explorer window. Successful tests will be displayed in green, while failed tests will be displayed in red. You can click on each test to view details about the results.

# Technical details

## Entities

In Domain-Driven Design (DDD), entities represent objects in the domain model that have a distinct identity and lifecycle. These entities encapsulate both data and behavior, and they are the building blocks of the domain model, representing concepts or things within the problem domain.

Key characteristics of DDD entities include:

- Identity: Entities have a unique identity that distinguishes them from other entities. This identity is typically represented by a unique identifier, such as a database primary key or a GUID.

- Mutability: Entities can change state over time, and they encapsulate behavior that allows them to transition between different states. This behavior is typically expressed through methods or operations defined on the entity.

- Aggregates: Entities are often organized into aggregates, which are clusters of related entities that are treated as a single unit for the purpose of data consistency and transactional boundaries.

- Rich Behavior: Entities encapsulate behavior that is meaningful within the context of the domain. This behavior is expressed through methods or operations that operate on the entity's data.

- Persistence Ignorance: DDD entities are typically designed to be persistence ignorant, meaning they are not tightly coupled to the underlying data storage mechanism. Instead, they focus on representing domain concepts and behavior.

Overall, DDD entities play a central role in modeling the problem domain within a software application. By focusing on capturing the essential concepts and behavior of the domain, entities help to create a rich and expressive domain model that closely aligns with the needs and requirements of the business.

## Domain Services:

Domain Services are a part of the domain layer in Domain-Driven Design (DDD). They encapsulate domain-specific logic and behavior that does not naturally fit within the entities or value objects of the domain model.

Key characteristics of Domain Services include:

- Encapsulation of Domain Logic: Domain Services encapsulate domain-specific logic and behavior that cannot be directly attributed to entities or value objects. This can include operations that involve multiple entities or complex business rules.

- Statelessness: Domain Services are typically stateless, meaning they do not maintain any state between method invocations. They operate solely on the input provided to them and produce output based on that input.

- Domain Expertise: Domain Services are designed to encapsulate the expertise and knowledge of the domain experts, ensuring that domain-specific logic is captured and implemented correctly.
Collaboration with Entities and Value Objects: Domain Services often collaborate with entities and value objects to perform their operations. They may delegate tasks to entities or value objects and coordinate their interactions to achieve a specific outcome.

Overall, Application Services and Domain Services play complementary roles in software architecture, with Application Services acting as a bridge between the presentation and domain layers, and Domain Services encapsulating domain-specific logic within the domain layer. Together, they help ensure that the application's business logic is properly encapsulated, organized, and executed in a maintainable and scalable manner.

## Application Services

Application Services, also known as Application Facades or Application Layers, are a part of the service layer in software architecture. They serve as an interface between the presentation layer and the domain layer, encapsulating the application's business logic and coordinating the execution of use cases.

Key characteristics of Application Services include:

- Use Case Coordination: Application Services orchestrate the execution of use cases by invoking domain logic, coordinating transactions, and managing the flow of control within the application.
Transaction Management: Application Services are responsible for managing transactions and ensuring that changes to the system's state are made atomically and consistently.

- Input Validation: Application Services validate input data received from the presentation layer before passing it to the domain layer, ensuring that only valid data is processed.

- Presentation-Agnostic: Application Services are presentation-agnostic, meaning they are not tied to any specific user interface technology. They can be reused across different presentation layers, such as web, mobile, or desktop applications.

## DTOs

Data Transfer Objects (DTOs) are a design pattern commonly used in software development to transfer data between different layers of an application or between different parts of a distributed system. DTOs serve as lightweight, serializable objects that carry data without behavior or business logic.

Key characteristics of DTOs include:

- Data Structure: DTOs typically consist of a set of properties that represent the data being transferred. These properties mirror the structure of the data being transferred but do not contain any behavior or methods.

- Serialization: DTOs are designed to be serializable, meaning they can be easily converted into a format that can be transmitted over a network or stored in a persistent storage system. This makes them suitable for use in distributed systems or when communicating between different layers of an application.

- Decoupling: DTOs help decouple the representation of data from the internal implementation details of the application. By defining a separate set of DTOs for data transfer, developers can isolate changes in the data structure from the rest of the application.

- Transformation: DTOs are often used to transform data between different formats or representations. For example, they can be used to map data from database entities to a format that is more suitable for presentation in a user interface.

- Reduced Overhead: DTOs help reduce the amount of data being transferred between different parts of the application by including only the necessary information. This can help improve performance and reduce network bandwidth usage.

Overall, DTOs are a useful tool for facilitating data transfer between different parts of a software application or between different systems. By providing a standardized, serializable representation of data, DTOs help improve interoperability, decouple components, and simplify the integration of disparate systems.

## Repository

The Repository pattern is a design pattern commonly used in software development to abstract and encapsulate the logic for accessing and managing data. It provides a layer of separation between the application's business logic and the underlying data access mechanisms, such as databases or web services. In the Repository pattern, data access logic is encapsulated within specialized classes called repositories. These repositories act as a middleman between the application and the data storage, providing a set of methods for performing common CRUD (Create, Read, Update, Delete) operations on the data. The main benefits of using the Repository pattern include: 

- Abstraction of Data Access: Repositories abstract the details of how data is stored and retrieved, allowing the rest of the application to work with a consistent interface regardless of the underlying data source. 

- Improved Testability: By encapsulating data access logic within repositories, it becomes easier to write unit tests for the application's business logic without needing to access the actual data storage. 

- Flexibility and Maintainability: The Repository pattern promotes a modular and maintainable codebase by separating concerns and providing a clear separation of responsibilities between different layers of the application. 

- Promotion of Best Practices: Repositories encourage the use of best practices such as dependency injection and separation of concerns, leading to cleaner and more maintainable code.

Overall, the Repository pattern is a powerful tool for managing data access in software applications, providing benefits such as abstraction, testability, flexibility, and maintainability. It is widely used in modern software development, particularly in applications built on top of frameworks like ASP.NET Core and Entity Framework.

## IOC

IOC, or Inversion of Control, is a design principle in software engineering where the control over the flow of a program's execution is inverted or transferred from the program itself to an external framework or container. In other words, rather than a component or class controlling the instantiation or management of its dependencies, IOC delegates this responsibility to an external entity.The main goal of IOC is to promote loose coupling between components, making the code more modular, reusable, and easier to maintain. This is typically achieved by using dependency injection, a common technique in IOC, where dependencies are injected into a class or component rather than being instantiated directly within it. By adopting IOC, developers can write code that is more flexible and easier to test, as dependencies can be easily replaced or mocked during unit testing. IOC containers, such as Microsoft's built-in Dependency Injection container in ASP.NET Core, help manage the instantiation and resolution of dependencies, further simplifying the development process. Overall, IOC is a powerful design principle that promotes better software architecture by decoupling components and improving code maintainability and testability.