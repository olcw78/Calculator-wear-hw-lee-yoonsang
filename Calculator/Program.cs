// 예제 표현식들
var givenExpressions = new[] {
    "1 + 2 * 3", 
    "(1 + 2) * 3", 
    "1 / 32.5 + 167 * (3498 - 1155) * -721 * (4885 - 1) / 0.5",
    "sin(cos(1)) * cos(1)"
};

var calculator = new Calculator();
foreach (double e in givenExpressions.Select(calculator.Evaluate))
    Console.WriteLine(e);