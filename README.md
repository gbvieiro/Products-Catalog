# Products-Catalog

Welcome to the product catalog. 
This project was developed to demostrate a example of a Clean Architecture using .NET 8 and bases on Domain Driven Design guides.
Here we will control a book catalog, creating orders and controling the stock of this items.

# Prerequisites

- Visual Studio 2022

# Executing

1. Open .sln file 'src\Products.Catalog.Domain.sln' with Visual Studio 2022
2. Set the project 'Product.Catalog.API' as starup project and execute using 'F5'
3. When the project is opened you can type a "F5" to execute
4. A URI will be opened in your browser, please allow Visual Studio to perform necessary actions.
5. Select a method to execute and expand the component for more information.
6. Click on 'Try it Out' and fill requested parameters.
7. Take a look in results.

-- common issues

- This project has a authentication control, so a login is required to execute methods. Use the "Authorize button to inform a token".


# How-to

Here I will share some steps to simulate this API basic operations:

1. Create user:

Use the "api/User/Save" method to create a admin User: 

```javascript
    {
        "email": "admin@gmail.com",
        "password": "MyPassword",
        "role": "admin"
    }
```

The new user id is returned.


Use the "api/User/Save" method to create a client User: 

```javascript
    {
        "email": "client@gmail.com",
        "password": "MyPassword",
        "role": "client"
    }
```

2. using the "api/Authentication/GenerateToken" to Generate a token to the admin user:

```javascript
    {
        "email": "admin@gmail.com",
        "password": "MyPassword"
    }
```

A token will be returned, copy it to a notepad.

3. Using the "Authorize" button, inform the returned token using this format:

```javascript
    Bearer <your-token-here>
```

- Remember to click on the "Authorize" button in the modal to confirm operation.
- Now you are authenticated!
- Close the modal.

4. Confirm that you are logged in using the "/api/0Authentication/GetCurrentUserInfo" method to get your user information.

5. Create a book using the "/api/Books/Save" method to create a book:

```javascript
    {
        "price": 22.50,
        "title": "The Shining",
        "author": "Stephen King",
        "genre": 10 // Horror
    }
```

The new book will be returned with the new ID. Copy this id for the next step.

6. Confirm that your new book was added using the "/api/Books/{id}/AddBooks" method informing the book Id on the field and a quantity of items in the body:

```javascript
    {
        "quantity": 10
    }
```

7. Use the "/api/Stocks/book/{bookId}" method to confirm that stock quantity is updated.

8. Use the "/api/Orders/Save" method to create a new order for your custumer user:

```javascript
    
    {
        "customerId": "{your-client-user-id-here}",
        "creationData": "2024-06-01T04:28:55.493Z", // Modify for a current data
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

9. Perform a logout clicking in the "Authorize" button again.

- When the modal is showed click on "Logoff"

10. Using the "api/Authentication/GenerateToken" to Generate a token to the client user:

```javascript
    {
        "email": "client@gmail.com",
        "password": "MyPassword"
    }
```

A token will be returned, copy it to a notepad.

11. Using the "Authorize" button, inform the returned token using this format:

```javascript
    Bearer <your-token-here>
```

- Remember to click on the "Authorize" button in the modal to confirm operation.
- Now you are authenticated!
- Close the modal.

12. Use the "/api/Orders/MyOrders" method to see current client user orders. This is a paginated API so we can set
parameters with these values:

Text: ""
skip: 0 // first page
Take: 10 // first 10 items

- The created order must be returned.
- Copy the order Id.

13. Use the "/api/Orders/{id}" method to see register order details, informing the order id.

14. Login with the Admin user again and cancel the order using "/api/Orders/{orderId}/cancel" method, informing the order id.

15. Use the "/api/Stocks/book/{bookId}" method to confirm that stock quantity is updated.

