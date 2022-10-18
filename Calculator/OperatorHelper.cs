public static class OperatorHelper
{
    static readonly string[] OperatorCaches = {"+", "-", "/", "*"};
    static readonly string[] TrigonometricOperatorCaches = {"sin", "cos"};

    public static bool Validate(string token,
        IEnumerable<Func<string, bool>> validators) =>
            validators
                   .Select(validator => validator(token))
                   .Aggregate((p, n) => p | n);

    public static bool IsOperator(string compared) =>
            OperatorCaches.Any(x => x == compared);

    public static bool IsTrigonometricOperator(string compared) =>
            TrigonometricOperatorCaches.Any(compared.Contains);

    public static int EvaluatePriority(string c) => c switch
    {
        "(" => 0,
        ")" => 0,
        "+" => 1,
        "-" => 1,
        "*" => 2,
        "/" => 2,
        "cos" => 3,
        "sin" => 3,
        _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
    };

    public static IEnumerable<(string str, bool isOperator)> SplitExpression(
        string expr,
        params Func<string, bool>[] operatorValidators
    )
    {
        foreach (string token in expr.Split(" ", StringSplitOptions.RemoveEmptyEntries))
        {
            bool isOperator = false;

            if (token.Contains('('))
            {
                yield return ("(", true);
                bool nestedParens = token.Count(x => x == ')') > 1;
                var enumerable = token.Split(new[] {'(', ')'}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string inner in enumerable)
                {
                    isOperator = Validate(inner, operatorValidators);
                    yield return (inner, isOperator);
                }

                if (nestedParens)
                    yield return (")", true);

                continue;
            }

            if (token.Contains(')'))
            {
                var enumerable = token.Split(new[] {'(', ')'}, StringSplitOptions.RemoveEmptyEntries);
                foreach (string inner in enumerable)
                {
                    isOperator = Validate(inner, operatorValidators);
                    yield return (inner, isOperator);
                }

                yield return (")", true);

                continue;
            }

            isOperator = Validate(token, operatorValidators);
            yield return (token, isOperator);
        }
    }
}
