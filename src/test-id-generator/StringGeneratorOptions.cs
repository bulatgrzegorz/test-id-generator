namespace test_id_generator;

public readonly ref struct StringGeneratorOptions
{
    private StringGeneratorOptions(char[] charChoices, int length)
    {
        CharChoices = charChoices;
        Length = length;
    }

    public static StringGeneratorOptions Create(char[] charChoices, int length = 16)
    {
        return new StringGeneratorOptions(charChoices, length);
    }

    public readonly int Length;
    public readonly char[] CharChoices;
}