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

  var apiResponse = Api.GetEnityById(id);
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
