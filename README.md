# DrSproc
Dr Sproc is a syntactically simple way to call SQL stored procedures!

The idea behind this library is to enable developers to write stored procedure calls in a syntatically similar way to how they are called in SQL server. Using a chained builder-like pattern it ensures you don't have to worry about SQL Connections and Commands. This should allow quick understanding of what's being called. It also aims to give clear error handling to ensure you don't have to spend too much time debugging through code to know which stored procedure, parameter or field is causing the problem.

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
The methods ```ReadInt``` and ```ReadNullableInt``` etc are built into the ```CustomMapper<>``` abstract class. For a full list of available options check here.

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


### Transactions


### Exceptions

See [this](https://github.com/jephbayf1986/DrSprocExampleProject) project for several examples of how to implement DrSproc.

## License, Copyright etc
Dr Sproc is created by Jeph & Georgina Bayfield and is licensed under the MIT license.
