namespace test_id_generator;

public readonly ref struct IntGeneratorOptions
{
    private IntGeneratorOptions(int minValue = 0, int maxValue = int.MaxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public static IntGeneratorOptions Create(int minValue = 0, int maxValue = int.MaxValue)
    {
        return new IntGeneratorOptions(minValue, maxValue);
    }

    public readonly int MinValue;
    public readonly int MaxValue;
}