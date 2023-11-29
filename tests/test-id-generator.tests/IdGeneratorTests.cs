namespace test_id_generator.tests;

public class IdGeneratorTests
{
    [Fact]
    public void GetId_SingleValue()
    {
        Assert.Equal(988942347, IdGenerator.GetId());
    }
    
    [Fact]
    public void GetId_SingleValue_MinMaxParameters()
    {
        Assert.Equal(1, IdGenerator.GetId(IntGeneratorOptions.Create(1, 4)));
    }
    
    [Fact]
    public void GetId_SingleValue_MinParameter()
    {
        Assert.Equal(1901734463, IdGenerator.GetId(IntGeneratorOptions.Create(6)));
    }
    
    [Fact]
    public void GetId_SingleValue_MaxParameter()
    {
        Assert.Equal(10, IdGenerator.GetId(IntGeneratorOptions.Create(maxValue: 12)));
    }
    
    [Fact]
    public void GetStringId_SingleValue()
    {
        Assert.Equal("Isn3BjHozZkFyWrd", IdGenerator.GetStringId());
    }
    
    [Fact]
    public void GetStringId_SingleValue_Length()
    {
        Assert.Equal(69, IdGenerator.GetStringId(lenght: 69).Length);
    }
    
    [Fact]
    public void GetStringId_SingleValue_CharChoices()
    {
        Assert.Equal("9abba99cccbaab9c", IdGenerator.GetStringId(StringGeneratorOptions.Create("abc69".ToCharArray())));
    }
    
    [Fact]
    public void GetStringId_SingleValue_CharChoices_Length()
    {
        Assert.Equal("6a9b", IdGenerator.GetStringId(StringGeneratorOptions.Create("abc69".ToCharArray(), 4)));
    }
    
    [Fact]
    public void IdGenerator_IntAndStringMixed()
    {
        Assert.Equal(1706861195, IdGenerator.GetId());
        Assert.Equal("bJKh7EJni5znZxpg", IdGenerator.GetStringId());
    }
}