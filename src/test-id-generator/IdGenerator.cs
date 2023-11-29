using System.Buffers;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace test_id_generator;

public static class IdGenerator
{
    private static readonly ConcurrentDictionary<string, Random> RandomCache = new();
    private static readonly char[] StringIdCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

    public static int GetId(string? type = null, [CallerFilePath] string currentTestFile = "", [CallerMemberName] string currentTestMethod = "")
    {
        var random = GetRandom(type, currentTestFile, currentTestMethod);
        return InternalGetIntId(random);
    }
    
    public static int GetId(IntGeneratorOptions options, string? type = null, [CallerFilePath] string currentTestFile = "", [CallerMemberName] string currentTestMethod = "")
    {
        var random = GetRandom(type, currentTestFile, currentTestMethod);
        return InternalGetIntId(random, options.MinValue, options.MaxValue);
    }
    
    public static int[] GetIds(int count, string? type = null, [CallerFilePath] string currentTestFile = "", [CallerMemberName] string currentTestMethod = "")
    {
        var random = GetRandom(type, currentTestFile, currentTestMethod);
        return InternalGetIntIds(random, count);
    }
    
    public static int[] GetIds(int count, IntGeneratorOptions options, string? type = null, [CallerFilePath] string currentTestFile = "", [CallerMemberName] string currentTestMethod = "")
    {
        var random = GetRandom(type, currentTestFile, currentTestMethod);
        return InternalGetIntIds(random, count, options.MinValue, options.MaxValue);
    }

    public static string GetStringId(int lenght = 16, string type = "", [CallerFilePath] string currentTestFile = "", [CallerMemberName] string currentTestMethod = "")
    {
        var random = GetRandom(type, currentTestFile, currentTestMethod);
        return InternalGetStringId(random, StringIdCharacters, lenght);
    }
    
    public static string GetStringId(StringGeneratorOptions options, string type = "", [CallerFilePath] string currentTestFile = "", [CallerMemberName] string currentTestMethod = "")
    {
        var random = GetRandom(type, currentTestFile, currentTestMethod);
        return InternalGetStringId(random, options.CharChoices, options.Length);
    }
    
    public static string[] GetStringIds(int count, int lenght = 16, string type = "", [CallerFilePath] string currentTestFile = "", [CallerMemberName] string currentTestMethod = "")
    {
        var random = GetRandom(type, currentTestFile, currentTestMethod);
        return InternalGetStringIds(random, count, StringIdCharacters, lenght);
    }
    
    public static string[] GetStringIds(int count, StringGeneratorOptions options, string type = "", [CallerFilePath] string currentTestFile = "", [CallerMemberName] string currentTestMethod = "")
    {
        var random = GetRandom(type, currentTestFile, currentTestMethod);
        return InternalGetStringIds(random, count, options.CharChoices, options.Length);
    }

    private static int[] InternalGetIntIds(Random random, int count, int minValue = 0, int maxValue = int.MaxValue)
    {
        var result = new int[count];
        for (var i = 0; i < count; i++)
        {
            result[i] = InternalGetIntId(random, minValue, maxValue);
        }

        return result;
    }
    
    private static int InternalGetIntId(Random random, int minValue = 0, int maxValue = int.MaxValue) => random.Next(minValue, maxValue);
    
    private static string[] InternalGetStringIds(Random random, int count, char[] choices, int length)
    {
        var result = new string[count];
        for (var i = 0; i < count; i++)
        {
            result[i] = InternalGetStringId(random, choices, length);
        }

        return result;
    }
    private static string InternalGetStringId(Random random, char[] choices, int length)
    {
        if (choices is not { Length: > 0 })
        {
            throw new ArgumentException("Choices must be not empty", nameof(choices));
        }

        if (length == 0)
        {
            return string.Empty;
        }
        
        var items = ArrayPool<char>.Shared.Rent(length);
        try
        {
            for (var i = 0; i < length; i++)
            {
                items[i] = choices[random.Next(choices.Length)];
            }

            return new string(items[..length]);
        }
        finally
        {
            ArrayPool<char>.Shared.Return(items);
        }
    }

    private static Random GetRandom(string? type, string currentTestFile, string currentTestMethod)
    {
        var fullType = GetFullType(currentTestFile, currentTestMethod, type ?? string.Empty);
        return RandomCache.GetOrAdd(fullType, s => new Random(GetIntValueBasedOnFullType(s)));
    }

    private static int GetIntValueBasedOnFullType(string fullType) => fullType.GetDeterministicHashCode();

    private static string GetFullType(string currentTestFile, string currentTestMethod, string type)
    {
        return $"{GetTopDirectoryName(currentTestFile)}.{Path.GetFileNameWithoutExtension(currentTestFile)}.{currentTestMethod}.{type}";
    }

    private static string GetTopDirectoryName(string path) => Path.GetFileName(Path.GetDirectoryName(path))!;

    // GetHashCode starting from .NET Core generates different value for each program execution (hash method salt is being randomized).
    // In our case, we need exact same value to be generated each time - otherwise at each tests execution identifiers will got different value.
    // As our generated identifiers are suppose to be use exclusively for tests, we should be safe to use deterministic hash codes.
    // copied from: https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/
    // originally based on: https://github.com/dotnet/corefx/blob/a10890f4ffe0fadf090c922578ba0e606ebdd16c/src/Common/src/System/Text/StringOrCharArray.cs#L140
    private static int GetDeterministicHashCode(this string str)
    {
        unchecked
        {
            var hash1 = (5381 << 16) + 5381;
            var hash2 = hash1;

            for (var i = 0; i < str.Length; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1)
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
    }
}
