using System.Runtime.InteropServices.ComTypes;

public class Calculator
{
    /// <summary>
    /// 계산식을 실행.
    /// </summary>
    /// <param name="expression">계산식</param>
    /// <returns>계산 결과</returns>
    public double Evaluate(string expression)
    {
        // 중위표현식 -> 후위표현식으로 변환.
        var postFix = ConvertToPostfix(expression);
        var result = new Stack<double>();

        // 후위 연산식을 순회하며 result 로 계산 후 삽입.
        // result 에는 피연산자만 존재하도록 처리.
        // sin, cos 는 단항 연산자, 이외는 이항 연산자로 보고 계산.
        foreach (string str in postFix)
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

    /// <summary>
    /// 중위 표현식 -> 후위 표현식으로 변환
    /// </summary>
    /// <param name="infixExpression">중위 표현식</param>
    /// <returns>후위 표현식 IEnumerable</returns>
    public IEnumerable<string> ConvertToPostfix(string infixExpression)
    {
        // 수식 쪼개서 IEnumerable 로 받기.
        var exprEnumerable = OperatorHelper.SplitExpression(
            infixExpression,
            OperatorHelper.IsOperator,
            OperatorHelper.IsTrigonometricOperator
        );

        // loop
        using var enumerator = exprEnumerable.GetEnumerator();

        var postFix = new List<string>();
        var operators = new Stack<string>();

        while (enumerator.MoveNext())
        {
            // destructure tuple
            (string str, bool isOp) = enumerator.Current;

            // 괄호 시작 -> 받을 때까지 내부 우선순위로 후위연산 구성 List, Stack 에 넣기.
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

                // 피연산자 List 에 붙이기 연산자 Stack 붙이기.
                postFix.AddRange(operators);
                operators.Clear();
                continue;
            }

            // 현재 enumerator 가 연산자일 때,
            // 현재 operators stack 의 top 인 연산자와 우선순위 비교 후 처리.
            if (isOp)
            {
                ComparePriorityRecursively(str, operators, postFix);
                continue;
            }

            // 연산자 아니면 그냥 postFix List 에 삽입.
            postFix.Add(str);
        }

        // expression iteration 완료 시에 operators 차례로 추가.
        postFix.AddRange(operators);
        return postFix;
    }


    /// <summary>
    /// 두 연산자들의 우선순위를 비교하여 재귀적으로 수식을 구성.
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

        // 이전 연산자 (operator stack 의 top) 가 새로운 연산자보다 우선순위가 높거나 같은 경우 -> postFix 로 pop.
        // 그리고 그렇지 않을 경우까지 재귀비교.
        if (prevPriority >= newPriority)
        {
            postfix.Add(operators.Pop());
            ComparePriorityRecursively(newChar, operators, postfix);
            return;
        }

        operators.Push(newChar);
    }
}
