namespace CalculatorTest;

public static class TestResources
{
    public static IEnumerable<string> TestExpressions => new[]
    {
        "1 + 2 * 3", "(1 + 2) * 3", "1 / 32.5 + 167 * (3498 - 1155) * -721 * (4885 - 1) / 0.5",
        "sin(cos(1)) * cos(1)"
    };

    public static IEnumerable<string[]> TestSplits => new[]
    {
        new[] {"1", "+", "2", "*", "3"}, new[] {"(", "1", "+", "2", ")", "*", "3"},
        new[]
        {
            "1", "/", "32.5", "+", "167", "*", "(", "3498", "-", "1155", ")", "*", "-721", "*", "(",
            "4885", "-", "1", ")", "/", "0.5"
        },
        new[] {"(", "sin", "cos", "1", ")", "*", "(", "cos", "1"}
    };

    public static IEnumerable<IEnumerable<string>> TestPostfixes => new[]
    {
        new[] {"1", "2", "3", "*", "+"}, new[] {"1", "2", "+", "3", "*"},
        new[]
        {
            "1", "32.5", "/", "167", "3498", "1155", "-", "*", "+", "-721", "*", "4885", "1", "-", "*",
            "0.5", "/"
        },
        new[] {"1", "cos", "sin", "1", "cos", "*"}
    };
    
    public static IEnumerable<double> TestExpectations => new[]
    {
        7, 
        9, 
        -2755685654567.969, // -2755685871267.3237 정확도 오류?
        0.2779289443079115
    };
}
