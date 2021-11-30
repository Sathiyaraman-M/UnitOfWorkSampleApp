# UnitOfWorkSampleApp
A Sample console app to demonstrate the use of "Unit Of Work" with repository pattern, along with Entity Framework Core.

The Console App has a `Program.cs` as entry point.

The `Book.cs` has two database model classes - `BookHeader` and `Book`

The `Common` folder has interfaces for the above model classes, which provides `Id` and some record history detail fields.

The `Repositories` folder has a `UnitOfWork` class that is used to get an instance of `IRepositoryAsync` to perform database operations. 
The class also provides methods to save changes to database or rollback them before they get saved.
It acts as a high-level wrapper to the `LibraryDbContext` class, but also prevents any unneccessary secondary DbContext transactions by using same instance of DbContext across everywhere.

In `Program.cs`, some `Book` and `BookHeader` objects are created and added to a database through an `UnitOfWork` object.

You can refer the code to understand the Unit of Work pattern.
Also, you can also add an interface for `UnitOfWork`, if you are intending to use it with dependency injection in your ASP.NET Core apps.
