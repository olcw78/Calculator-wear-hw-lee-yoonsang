namespace CalculatorTest;

public class Tests
{
    Calculator _calculator;

    IEnumerable<string> TestExpressions => new[]
    {
        "1 + 2 / 3 * 5 - 2", "(1 + 2) * 3", "1 / 32.5 + 167 * (3498 - 1155) * -721 * (4885 - 1) / 0.5",
        "sin(cos(1)) * cos(1)"
    };

    // IEnumerable<IEnumerable<char>> TestPostfix => new[] {new[] {'1', '2', '3', '*', '+'}, new[]
    // {
    //     ''
    // }};
    IEnumerable<double> TestExpectations => new[] {2.3333333333, 9, -2755685654567.969, 0.2779289443079115};

    [SetUp]
    public void Setup() { _calculator = new(); }

    [Test]
    public void OK_Split_Expressions()
    {
        foreach (string expr in TestExpressions)
        {
            var split = _calculator.SplitExpression(
                expr,
                OperatorHelper.IsOperator,
                OperatorHelper.IsTrigonometricOperator
            );

            foreach (var (c, isOperator) in split)
            {
                Assert.That(
                    isOperator
                            ? OperatorHelper.IsOperator(c) || OperatorHelper.IsTrigonometricOperator(c)
                            : int.TryParse(c.ToString(), out var _)
                );
            }
        }
    }

    [Test]
    public void OK_Convert_Expression_To_Postfix_Notation()
    {
        foreach (string expr in TestExpressions)
        {
            var postfix = _calculator.ConvertToPostfix(expr);
        }
    }

    [Test]
    public void OK_Calculate_Expression()
    {
        using var enumerator = TestExpectations.GetEnumerator();
        foreach (string expr in TestExpressions)
        {
            double result = _calculator.Evaluate(expr);
            if (enumerator.MoveNext())
            {
                var cur = enumerator.Current;
                var tolerance = result - cur;
                Assert.That(Math.Abs(tolerance) < 0.0000001);
            }
        }
    }
}
