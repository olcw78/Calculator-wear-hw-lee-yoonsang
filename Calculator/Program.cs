// See https://aka.ms/new-console-template for more information

var calculator = new Calculator();

var givenExpressions = new[]
{
    "1 + 2 / 3 * 5 - 2", "(1 + 2) * 3", "1 / 32.5 + 167 * (3498 - 1155) * -721 * (4885 - 1) / 0.5"
};

foreach (var e in givenExpressions.Select(calculator.Evaluate))
{
    Console.WriteLine(e);
}
