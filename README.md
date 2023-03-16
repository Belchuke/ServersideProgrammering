# How to start the project

1. Get the .env file that isn't commited to the repository for security. Place it in the backend folder.

2. The database runs via docker compose, so to start the container
```docker compose up```

3. The backend runs is in ASP.NET Core.
To setup the app, you need to setup the two databases first
```
Update-Database -Context ApiDbContext
Update-Database -Context ToDoDbContext
```
... or the `dotnet` equivalent
```
dotnet ef database update --context ApiDbContext
dotnet ef database update --context ToDoDbContext
```



To run the app, so open visual studio and run it from there, or use the `dotnet` command (within the backend folder):
```dotnet run```

4. The frontend is in ReactJS.
To install all the dependencies, you need `npm`, and run (within the frontend folder):
```npm install```
To run the frontend:
```npm start```

