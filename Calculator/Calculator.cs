using System.Runtime.InteropServices.ComTypes;

public class Calculator
{
    public double Evaluate(string expression)
    {
        var postFixEnumerable = ConvertToPostfix(expression);

        // 3. calculate the notation
        var result = new Stack<double>();

        foreach (string str in postFixEnumerable)
        {
            double n1;
            double n2;

            switch (str)
            {
                case "+":
                    _ = result.TryPop(out n1);
                    _ = result.TryPop(out n2);
                    result.Push(n2 + n1);
                    break;

                case "-":
                    _ = result.TryPop(out n1);
                    _ = result.TryPop(out n2);
                    result.Push(n2 - n1);
                    break;

                case "*":
                    _ = result.TryPop(out n1);
                    _ = result.TryPop(out n2);
                    result.Push(n2 * n1);
                    break;

                case "/":
                    _ = result.TryPop(out n1);
                    _ = result.TryPop(out n2);
                    result.Push(n2 / n1);
                    break;

                case "sin":
                    _ = result.TryPop(out n1);
                    result.Push(Math.Sin(n1));
                    break;

                case "cos":
                    _ = result.TryPop(out n1);
                    result.Push(Math.Cos(n1));
                    break;

                default:
                    if (double.TryParse(str, out double parsed))
                        result.Push(parsed);
                    break;
            }
        }

        return result.Pop();
    }

    public IEnumerable<string> ConvertToPostfix(string expression)
    {
        var postFix = new List<string>();
        var operators = new Stack<string>();

        var exprEnumerable = OperatorHelper.SplitExpression(
            expression,
            OperatorHelper.IsOperator,
            OperatorHelper.IsTrigonometricOperator
        );

        using var enumerator = exprEnumerable.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var (str, isOp) = enumerator.Current;

            if (str == "(")
            {
                while (enumerator.MoveNext())
                {
                    (str, isOp) = enumerator.Current;
                    if (str == ")")
                        break;

                    if (isOp)
                        operators.Push(str);
                    else
                        postFix.Add(str);
                }

                postFix.AddRange(operators);
                operators.Clear();

                continue;
            }

            if (isOp)
            {
                ComparePriorityRecursively(str, operators, postFix);
                continue;
            }

            postFix.Add(str);
        }

        postFix.AddRange(operators);

        return postFix;
    }


    /// <summary>
    /// 2 연산자의 우선순위를 비교하여 재귀적으로 수식을 구성.
    /// </summary>
    /// <param name="newChar">새로운 char</param>
    void ComparePriorityRecursively(string newChar, Stack<string> operators, List<string> postfix)
    {
        // 비교할 연산자가 아얘 없을 경우
        if (operators.Count == 0)
        {
            operators.Push(newChar);
            return;
        }

        string last = operators.Peek();
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
