var calculator = new Calculator();

var givenExpressions = new[]
{
    "1 + 2 * 3", "(1 + 2) * 3", "1 / 32.5 + 167 * (3498 - 1155) * -721 * (4885 - 1) / 0.5",
    "sin(cos(1)) * cos(1)"
};

// foreach (double e in givenExpressions.Select(calculator.Evaluate))
// {
//     Console.WriteLine(e);
// }


Console.WriteLine(calculator.Evaluate("1 / 32.5 + 167 * (3498 - 1155) * -721 * (4885 - 1) / 0.5"));
Console.WriteLine(calculator.Evaluate("sin(cos(1)) * cos(1)"));
