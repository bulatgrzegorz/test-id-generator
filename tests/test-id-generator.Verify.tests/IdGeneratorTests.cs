namespace test_id_generator.verify.tests;

[UsesVerify]
public class IdGeneratorTests
{
    [Fact]
    public Task IdGenerator_PersonVerifyTest()
    {
        return Verify(new Person(IdGenerator.GetId(), IdGenerator.GetStringId()));
    }
    
    [Fact]
    public Task IdGenerator_PersonVerifyTest_2()
    {
        return Verify(new Person(IdGenerator.GetId(), IdGenerator.GetStringId()));
    }

    [Fact]
    public Task IdGenerator_ManyIds()
    {
        return Verify(new { ints = IdGenerator.GetIds(3), strings = IdGenerator.GetStringIds(6) });
    }
}

public record Person(int Id, string CorrelationId);