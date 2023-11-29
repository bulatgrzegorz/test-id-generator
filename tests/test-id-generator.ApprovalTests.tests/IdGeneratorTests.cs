using System.Text.Json;
using ApprovalTests;
using ApprovalTests.Reporters;

namespace test_id_generator.ApprovalTests.tests;

[UseReporter(typeof(DiffReporter))]
public class IdGeneratorTests
{
    [Fact]
    public void IdGenerator_PersonVerifyTest()
    {
        Approvals.Verify(new Person(IdGenerator.GetId(), IdGenerator.GetStringId()));
    }
    
    [Fact]
    public void IdGenerator_PersonVerifyTest_2()
    {
        Approvals.Verify(new Person(IdGenerator.GetId(), IdGenerator.GetStringId()));
    }
    
    [Fact]
    public void IdGenerator_ManyIds()
    {
        Approvals.VerifyJson(JsonSerializer.Serialize(new { ints = IdGenerator.GetIds(3), strings = IdGenerator.GetStringIds(6) }));
    }
}

public record Person(int Id, string CorrelationId);