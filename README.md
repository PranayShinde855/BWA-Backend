Backend Architecture (.NET Core 8.0)

User an create blogs and edits of their own where as the admin can create blog and perform CRUD on it.
There are only two modules blogs and user. Except that the user can change password as per thire need.

The backend follows a clean architectural pattern using Repository Pattern with Unit of Work, SOLID principles ensuring separation of concerns and testability. 
•	Authentication & Authorization: Implemented using JWT (JSON Web Tokens).  Roles (Admin, User) are embedded in the token claims for access control.
•	Entity Framework Core: Utilizes eager loading for efficient data retrieval and manages database interactions. 
•	Middlewares: Includes Exception Handling Middleware for global error management and Content Security Policy (CSP) middleware for enhanced security.
•	Action Filters: Used for cross-cutting concerns like logging or request validation.
•	AutoMapper: Facilitates seamless object-to-object mapping between entities and DTOs.
