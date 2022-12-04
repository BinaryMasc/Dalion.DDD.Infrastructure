# Dalion.DDD.Infrastructure

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://travis-ci.org/joemccann/dillinger)

## About
Infrastructure for backend in C# .Net - Dynamic mapping using reflection, low code ORM, etc.

## Query to SQL 
```cs

//  Declare a QueryGenericHandler<T> Object
var usr = new User(); //    In this example we can use User class
var query = new QueryGenericHandler<User>(usr);

//  Set wheres and other filters
query.Where(u => u.Username == "UserTest" && u.IsActive);

//  Then execute the query
var result = await query.RunQuery()?[0];    //  Get the first result
```
The QueryGenericHandler<T> object build a query use the name of the class and its fields to build a query, assuming a table or view exists that matches the class and its fields and will automatically be mapped at runtime.

## Command Insert
```cs
//  User model example
User usr = new User
{
    Username = "UserFromApi",
    Email = "userfromApi@yahoo.cool",
    Name = "Big",
    LastName = "Mom",
    Profession = "dev",
    IsActive = true,
    IsBlocked = false,
};
var cmd = new CommandGenericHandler<User>(usr);
//  Insert to the User table
await cmd.RunInsert(usr);
```

## Configure ConnectionString
```cs
//	On startup.cs
SQLConnection.SetConnectionString("Server=localhost;Database=DalionDev;Integrated Security=SSPI");
```




## License

MIT

**Free Software, Hell Yeah!**

