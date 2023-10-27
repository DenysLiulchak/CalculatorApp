namespace CalculatorApp
{
    public enum ErrorCodeEnum : sbyte
    {
        InvalidStartIndex = -3,           // Неправильний індекс першого символу
        InvalidEndIndex = -2,             // Неправильний індекс останнього символу
        EmptyExpression = -1,             // expression.Length == 0
        NullExpression = 0,               // expression == null
        NoErrors = 1,                     // Немає помилок
        InvalidSeparator = 2,             // Розділювач дробових чисел поставлено неправильно
        NoOperand = 3,                    // Пропущено операнд
        NoOpeningBracket = 4,             // Пропущено відкриваючу дужку
        NoConstant = 5,                   // Не існує константи з такою назвою
        NoFunction = 6,                   // Не існує функцій з такою назвою
        EmptyBrackets = 7,                // Порожні дужки
        InvalidData = 8,                  // Неправильні дані(нематематичний символ)
        NoClosingBracket = 9,             // Пропущено закриваючу дужку
        NoOperator = 10                   // Пропощено оператор
    }
}
