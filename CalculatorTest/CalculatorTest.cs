namespace CalculatorTest;

public class CalculatorTest
{
    readonly Calculator _calculator = new Calculator();

    [Test]
    public void OK_Split_Expressions()
    {
        using var testEnumerator = TestResources.TestSplits.GetEnumerator();
        foreach (string expr in TestResources.TestExpressions)
        {
            var split = OperatorHelper.SplitExpression(
                        expr,
                        OperatorHelper.IsOperator,
                        OperatorHelper.IsTrigonometricOperator
                    ).Select(tuple => tuple.str)
                   .ToArray();

            if (testEnumerator.MoveNext())
                Assert.That(split.SequenceEqual(testEnumerator.Current));
        }
    }

    [Test]
    public void OK_Convert_Expression_To_Postfix_Notation()
    {
        using var exprEnumerater = TestResources.TestExpressions.GetEnumerator();
        using var postfixEnumerator = TestResources.TestPostfixes.GetEnumerator();

        while (exprEnumerater.MoveNext() &&
               postfixEnumerator.MoveNext())
        {
            var curExpr = exprEnumerater.Current;
            var curPostfix = postfixEnumerator.Current;

            using var innerExpr = _calculator.ConvertToPostfix(curExpr).GetEnumerator();
            using var innerPostfix = curPostfix.GetEnumerator();
            while (innerExpr.MoveNext() &&
                   innerPostfix.MoveNext())
            {
                var innerCurExpr = innerExpr.Current;
                var innerCurPostfix = innerPostfix.Current;

                Assert.AreEqual(innerCurExpr, innerCurPostfix);
            }
        }
    }

    [Test]
    public void OK_Calculate_Expression()
    {
        using var enumerator = TestResources.TestExpectations.GetEnumerator();

        foreach (string expr in TestResources.TestExpressions)
        {
            if (!enumerator.MoveNext())
                continue;

            double cur = enumerator.Current;
            double result = _calculator.Evaluate(expr);

            double tolerance = result - cur;
            Assert.That(Math.Abs(tolerance) < 0.0001);
        }
    }
}
