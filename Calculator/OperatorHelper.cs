public static class OperatorHelper
{
    static readonly char[] OperatorCaches = {'+', '-', '/', '*', '(', ')'};
    static readonly string[] TrigonometricOperatorCaches = {"sin", "cos"};

    public static bool IsOperator(char compared) => OperatorCaches.Any(x => x == compared);

    public static bool IsTrigonometricOperator(char compared) =>
            TrigonometricOperatorCaches.Any(x => x == compared.ToString());

    public static int EvaluatePriority(char c) => c switch
    {
        '(' => 0,
        ')' => 0,
        '+' => 1,
        '-' => 1,
        '*' => 2,
        '/' => 2,
        _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
    };
}
