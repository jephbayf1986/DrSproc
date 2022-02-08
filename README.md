# DrSproc
Dr Sproc is a syntactically simple way to call SQL stored procedures!

The idea behind this library is to enable developers to write stored procedure calls in a syntatically similar way to how they are called in SQL server. Using a chained builder-like pattern it ensures you don't have to worry about SQL Connections and Commands. This should allow quick understanding of what's being called. It also aims to give clear error handling to ensure you don't have to spend too much time debugging through code to know which stored procedure, parameter or field is causing the problem.

### Installation

You can install Dr Sproc from Nuget Package Manager, or Nuget CLI:

```cli
nuget install DrSproc
```

Or you can use the dotnet cli:

```cli
dotnet add package DrSproc
```

## How to Use
Before using *Dr Sproc* to call stored procedures the minimum setup required is to declare any Databases you may use, inheriting from the ```IDatabase``` interface:
```cs
// Declare the Databases ensuring they can fetch the connection string*
public class ContosoDb : IDatabase
{
    public string GetConnectionString()
    {
      return ConfigurationManager.ConnectionStrings["DrSprocTest"].ConnectionString;
    }
}
```

Now you can use this database to create a target database and build up a stored procedure call:
```cs
var db = DoctorSproc.Use<ContosoDb>();

db.Execute("sp_LogEvent")
            .Go();
```
You can also specify the schema if necessary:
```cs
var db = DoctorSproc.Use<ContosoDb>();

db.Execute("events", "sp_LogEvent")
            .Go();
```

### Parameters

After calling the execute method, you can declare the parameters you need using **WithParam** and **WithParamIfNotNull** methods.

```cs
var db = DoctorSproc.Use<ContosoDb>();

db.Execute("sp_UpdateDepartment")
            .WithParam("@DepartmentId", department.Id)
            .WithParam("@DepartmentName", department.Name)
            .WithParamIfNotNull("@AlternativeName", department.AltName)
                .Go();
```
*Note: the @ sign is not required in the declaration of the parameter name, so either the above or below would result in an identical procedure call:*

```cs
DoctorSproc.Use<ContosoDb>()
           .Execute("sp_UpdateDepartment")
                .WithParam("DepartmentId", department.Id)
                .WithParam("DepartmentName", department.Name)
                .WithParamIfNotNull("AlternativeName", department.AltName)
                    .Go();
```
### Return Types
There are 4 return types possible to be set after the parameter declarations:
 - Fire and forget, no return value *(This is the default, by calling ```Go()``` after declaring the parameters the procedure call will take place)*
 - Indentity Return Object
 - Single Return Type
 - Mulit Return Type (```IEnumerable``` output).

The return type, if any can be set as follows:

```cs
var id = DoctorSproc.Use<ContosoDb>()
                        .Execute("sp_CreateEmployee")
                            .WithParam("FirstName", mainItem.FirstName)
                            .WithParam("LastName", mainItem.LastName)
                                .ReturnIdentity()
                                    .Go();
```

```cs       
return DoctorSproc.Use<ContosoDb>()
                    .Execute("sp_GetEmployee")
                        .WithParam("EmployeeId", id)
                            .ReturnSingle<Employee>()
                                .Go();
```

```cs
return DoctorSproc.Use<ContosoDb>()
                    .Execute("sp_GetEmployees")
                        .ReturnMulti<Employee>()
                            .Go();
```

### Mapping
By default, the ```ReturnSingle<>``` and ```ReturnMulti<>``` calls stated above will use Reflection to obtain the model. But this has a performance cost and the return values may not align with the naming convention in the model being returned.

To create a mapper, inherit from ```CustomMapper<>``` and declare the field names and types as follows:

```cs
internal class EmployeeCustomMapper : CustomMapper<Employee>
{
    public override Employee Map()
    {
        return new Employee()
        {
            Id = ReadInt("Id"),
            FirstName = ReadString("FirstName"),
            LastName = ReadString("LastName"),
            DateOfBirth = ReadDateTime("DateOfBirth"),
            Department = new Department()
            {
                Id = ReadNullableInt("DepartmentId"),
                Name = ReadString("DepartmentName")
            }
        };
    }
}
```
The methods ```ReadInt``` and ```ReadNullableInt``` etc are built into the ```CustomMapper<>``` abstract class. Check [here](https://github.com/jephbayf1986/DrSproc/blob/main/src/DrSproc.App/EntityMapping/README.md) for a full list of available type reading options.

To use the mapper, simply declare it with ```UseCustomMapping<>()``` in the chain between the return type and the ```Go()``` as follows:

```cs       
return DoctorSproc.Use<ContosoDb>()
                    .Execute("sp_GetEmployee")
                        .WithParam("EmployeeId", id)
                            .ReturnSingle<Employee>()
                            .UseCustomMapping<EmployeeCustomMapper>()
                                .Go();
```

```cs
return DoctorSproc.Use<ContosoDb>()
                    .Execute("sp_GetEmployees")
                        .ReturnMulti<Employee>()
                        .UseCustomMapping<EmployeeCustomMapper>()
                            .Go();
```

### Timeout
To set a timeout for a synchronous stored procedure call, declare a timespan using ```WithTimeOut()``` at the same time as declaring the parameters:
```cs
var db = DoctorSproc.UseOptional<ContosoDb>(transaction);

db.Execute("sp_LongRunner")
            .WithParam("@Id", 45)
            .WithTimeOut(TimeSpan.FromSeconds(45))
                .Go();
```

### Asynchronous Calls
All the above calls can be done asynchronously. Instead of ```Execute()``` use ```ExecuteAsync()``` and this will give you ```GoAsync()``` instead of ```Go```. ```GoAsync()``` takes an optional CancellationToken as a parameter as follows:
```cs
var db = DoctorSproc.Use<ContosoDb>();

await db.ExecuteAsync("sp_UpdateDepartment")
            .WithParam("@DepartmentId", department.Id)
            .WithParam("@DepartmentName", department.Name)
            .WithParamIfNotNull("@AlternativeName", department.AltName)
                .GoAsync(token);
```
```cs
var id = await DoctorSproc.Use<ContosoDb>()
                        .ExecuteAsync("sp_CreateEmployee")
                            .WithParam("FirstName", mainItem.FirstName)
                            .WithParam("LastName", mainItem.LastName)
                                .ReturnIdentity()
                                    .GoAsync(token);
```
```cs       
return DoctorSproc.Use<ContosoDb>()
                    .ExecuteAsync("sp_GetEmployee")
                        .WithParam("EmployeeId", id)
                            .ReturnSingle<Employee>()
                            .UseCustomMapping<EmployeeCustomMapper>()
                                .GoAsync();
```

```cs
return DoctorSproc.Use<ContosoDb>()
                    .ExecuteAsync("sp_GetEmployees")
                        .ReturnMulti<Employee>()
                        .UseCustomMapping<EmployeeCustomMapper>()
                            .GoAsync();
```
*Note ```WithTimeOut()``` is not available for asynchronous calls*
### Dependency Injection
The examples throughout this readme all use the static DoctorSproc class. However Dependency Injection is also available...

Dr Sproc has 2 extension libraries to register dependency injection:
 - [DrSproc.DependencyInjection](https://www.nuget.org/packages/DrSproc.DependencyInjection/) for the Microsoft Dependency Injection library
 - [DrSproc.Unity](https://www.nuget.org/packages/DrSproc.Unity/) for Unity Container

For both the above, Dr Sproc is registered using the extension ```RegisterDrSproc()```.

To inject Dr Sproc use the ```ISqlConnector``` interface as follows:

```cs
private readonly ISqlConnector connector;

public DepartmentRepository(ISqlConnector connector)
{
    this.connector = connector;
}

public IEnumerable<Department> GetDepartments()
{
    var db = connector.Use<ContosoDb>();

    return db.Execute("sp_GetDepartments")
                    .ReturnMulti<Department>()
                        .Go();
}
```

### Transactions
Several options are built into Dr Sproc for using single database Transactions using the ```ITransaction``` interface.

To create and begin a transaction for a specific database, you can use the following command:
```cs
var contosoTransaction = DoctorSproc.BeginTransaction<ContosoDb>()
```
Then to execute a stored procedure within that transaction, you can use the following:
```cs
var dbTransaction = DoctorSproc.Use(transaction);

dbTransaction.Execute("sp_InsideTransaction")
                .Go();
```

For a full explanation of Transactions within this library, click [here](https://github.com/jephbayf1986/DrSproc/blob/main/src/DrSproc.App/Transactions/README.md)

### Exceptions
Dr Sproc has 3 custom exceptions to give you clear information when somethings gone wrong:
- ```DrSprocEntityMappingException```
- ```DrSprocNullReturnException```
- ```DrSprocParameterException```

All 3 will fully state the name of the stored procedure being called and detailed reasons for the error. For example if a null value is returned for a non-nullable property, you will see the following error:
```
The following error occurred while Dr Sproc attempted to read a non-null return object from sproc 'sp_GetThings': The data returned in field ThingId was of null, but a non-null value was expected
```
****

See [this](https://github.com/jephbayf1986/DrSprocExampleProject) project for several examples of how to implement DrSproc.

## License, Copyright etc
Dr Sproc is created by Jeph & Georgina Bayfield and is licensed under the MIT license.
