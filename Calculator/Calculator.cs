public class Calculator
{
#region Behaviour

    public IEnumerable<(char c, bool isOperator)> SplitExpression(
        string expr,
        params Func<char, bool>[] operatorValidators
    )
    {
        foreach (char c in expr)
        {
            if (c == ' ')
                continue;

            yield return (
                c,
                operatorValidators
                       .Select(validator => validator(c))
                       .Aggregate((p, n) => p | n)
            );
        }
    }

    public IEnumerable<char> ConvertToPostfix(string expression)
    {
        // 2. put all of the elements into the stack to configure in postfix notation.
        var exprEnumerable = SplitExpression(
            expression,
            OperatorHelper.IsOperator,
            OperatorHelper.IsTrigonometricOperator
        );

        var postFix = new List<char>();
        var operators = new Stack<char>();

        using var enumerator = exprEnumerable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var (c, isOp) = enumerator.Current;

            if (c == '(')
            {
                while (enumerator.MoveNext())
                {
                    (c, isOp) = enumerator.Current;
                    if (c == ')')
                        break;

                    if (isOp)
                        operators.Push(c);
                    else
                        postFix.Add(c);
                }
            }

            if (isOp)
            {
                ComparePriorityRecursively(c, operators, postFix);
                continue;
            }

            postFix.Add(c);
        }

        postFix.AddRange(operators);

        return postFix;
    }

    public double Evaluate(string expression)
    {
        var postFix = ConvertToPostfix(expression);

        // 3. calculate the notation
        var result = new Stack<double>();

        foreach (char c in postFix)
        {
            double n1 = 0.0;
            double n2 = 0.0;

            switch (c)
            {
                case '+':
                    _ = result.TryPop(out n1);
                    _ = result.TryPop(out n2);
                    result.Push(n2 + n1);
                    break;

                case '-':
                    _ = result.TryPop(out n1);
                    _ = result.TryPop(out n2);
                    result.Push(n2 - n1);
                    break;

                case '*':
                    _ = result.TryPop(out n1);
                    _ = result.TryPop(out n2);
                    result.Push(n2 * n1);
                    break;

                case '/':
                    _ = result.TryPop(out n1);
                    _ = result.TryPop(out n2);
                    result.Push(n2 / n1);
                    break;

                default:
                    if (double.TryParse(c.ToString(), out double parsed))
                        result.Push(parsed);
                    break;
            }
        }

        return result.Pop();
    }

#endregion Behaviour

    /// <summary>
    /// 2 연산자의 우선순위를 비교하여 재귀적으로 수식을 구성.
    /// </summary>
    /// <param name="newChar">새로운 char</param>
    void ComparePriorityRecursively(char newChar, Stack<char> operators, List<char> postfix)
    {
        // 비교할 연산자가 아얘 없을 경우
        if (operators.Count == 0)
        {
            operators.Push(newChar);
            return;
        }

        char last = operators.Peek();
        int prevPriority = OperatorHelper.EvaluatePriority(last);
        int newPriority = OperatorHelper.EvaluatePriority(newChar);

        // 이전 연산자가 새로운 연산자보다 우선순위가 높을 경우.
        if (prevPriority >= newPriority)
        {
            postfix.Add(operators.Pop());

            // 이전 연산자들이 더이상 새로운 연산자보다 우선순위가 높지 않을 때까지 재귀.
            ComparePriorityRecursively(newChar, operators, postfix);

            return;
        }

        operators.Push(newChar);
    }
}
