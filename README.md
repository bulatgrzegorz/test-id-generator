# TestIdGenerator

It's a library that allows you to generate deterministic random identifiers for your tests.

# Motivation
Motivation for this library was to make snapshot tests easier when need of usage random values.

Example of it would be multiple parallel tests running against web api process. In situation like this you might want each test to work with it's own entity.
```csharp
[Fact]
public Task CreatedEntity_WillBePosibleToBeQueriedAfter()
{
  var id = Random.Shared.Next();
  var entity = new Entity(id);
  Api.Create(entity);

  var apiResponse = Api.GetEntityById(id);
  return Verify(apiResponse);
}
```

Such snapshot will contain our random generated identifier, that will be different each time we run our test.
What you can do is ignore random property during verification:
```csharp
return Verify(apiResponse).IgnoreMember<Entity>(x => x.Id);
```

It will work fine, but it's requiring two steps to keep your mind around - generating value and ignoring it on verify step.
How we could do better is using TestIdGenerator:
```diff
- var id = Random.Shared.Next();
+ var id = IdGenerator.GetId();
```

Using it, `id` will be generated randomly but same each time for this test. Thanks to it, we can verify our entity object with used identifier in it, without need to ignore enything.

# Internals

`IdGenerator` is using runtime attributes [CallerFilePath](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callerfilepathattribute?view=net-8.0) and [CallerMemberName](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute?view=net-8.0)
