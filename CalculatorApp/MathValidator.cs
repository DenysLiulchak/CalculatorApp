using System.Globalization;

namespace CalculatorApp
{
    public class MathValidator      // Додати класи для операторів, змінних і, можливо, констант та використовувати їх там, де це потрібно
    {
        private readonly char decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        private readonly char[] binOperatorsArr = new char[]
        {
            '+',
            '-',
            '*',
            '/',
            '^'
        };          // Бінарні інфіксні оператори
        private readonly char[] postfixOperatorsArr = new char[]
        {
            '%',
            '!'
        };      // Унарні постфіксні оператори
        private readonly MathConstants constants = new MathConstants();   // Іменовані константи
        private readonly string[] functionsArr = new string[]
        {
            "sqrt",
            "sin",
            "cos",
            "tg",
            "ctg",
            "sec",
            "cosec",
            "ln",
            "log",
            "abs",
            "neg",
            "exp",
            "arcsin",
            "arccos",
            "arctg",
            "arcctg",
            "arcsec",
            "arccosec",
            "sign"
        };         // Унарні префіксні функції

        public char DecimalSeparator
        {
            get
            {
                return decimalSeparator;
            }
        }
        public char[] BinOperators
        {
            get
            {
                int length = binOperatorsArr.Length;
                char[] result = new char[length];

                for (int i = 0; i < length; ++i)
                    result[i] = binOperatorsArr[i];

                return result;
            }
        }
        public char[] PostfixOperators
        {
            get
            {
                int length = postfixOperatorsArr.Length;
                char[] result = new char[length];

                for (int i = 0; i < length; ++i)
                    result[i] = postfixOperatorsArr[i];

                return result;
            }
        }
        public MathConstants Constants
        {
            get
            {
                return new MathConstants(constants);
            }
        }
        public string[] Functions
        {
            get
            {
                int length = functionsArr.Length;
                string[] result = new string[length];

                for (int i = 0; i < length; ++i)
                    result[i] = functionsArr[i];

                return result;
            }
        }

        public MathValidator() { }
        public MathValidator(char decimalSeparator, char[] binOperators, char[] postfixOperators, MathConstants constants, string[] functions)
        {
            this.decimalSeparator = decimalSeparator;

            int binOperatorsLen = binOperators.Length;
            int postfixOperatorsLen = postfixOperators.Length;
            int functionsLen = functions.Length;

            binOperatorsArr = new char[binOperators.Length];
            postfixOperatorsArr = new char[postfixOperators.Length];
            functionsArr = new string[functions.Length];

            for (int i = 0; i < binOperatorsLen; ++i)
                binOperatorsArr[i] = binOperators[i];

            for (int i = 0; i < postfixOperatorsLen; ++i)
                postfixOperatorsArr[i] = postfixOperators[i];

            for (int i = 0; i < functionsLen; ++i)
                functionsArr[i] = functions[i];

            this.constants = new MathConstants(constants);
        }

        public bool IsBracket(char symbol)
        {
            return symbol == '(' || symbol == ')';
        }
        public bool IsDecimalSeparator(char symbol)
        {
            return symbol == decimalSeparator;
        }
        public bool IsBinOperator(char symbol)
        {
            foreach (char oper in binOperatorsArr)
                if (symbol == oper)
                    return true;

            return false;
        }
        public bool IsPostfixOperator(char symbol)
        {
            foreach (char oper in postfixOperatorsArr)
                if (symbol == oper)
                    return true;

            return false;
        }
        public bool IsConstant(string str)
        {
            return constants.IsConstant(str);
        }
        public bool IsFunction(string str)
        {
            foreach (string function in functionsArr)
                if (str == function)
                    return true;

            return false;
        }

        private ErrorCode CheckArgs(string expression, int startIndex, int endIndex)
        {
            if (expression == null)
                return new ErrorCode(ErrorCodeEnum.NullExpression, "Немає посилання на вираз");

            int length = expression.Length;

            if (length == 0)
                return new ErrorCode(ErrorCodeEnum.EmptyExpression, "Пустий вираз");

            if (startIndex < 0 || startIndex > endIndex || startIndex >= length)
                return new ErrorCode(ErrorCodeEnum.InvalidStartIndex, "Неправильний індекс першого символу");

            if (endIndex < 0 || endIndex >= length)
                return new ErrorCode(ErrorCodeEnum.InvalidEndIndex, "Неправильний індекс останнього символу");

            return new ErrorCode();
        }

        public ErrorCode CheckExpression(string expression, int startIndex, int endIndex)  // Перевіряє, чи expression — це математичний вираз
        {
            ErrorCode checkArgs = CheckArgs(expression, startIndex, endIndex);

            if (checkArgs.Code != ErrorCodeEnum.NoErrors)
                return checkArgs;

            char currentSymbol;
            string substr;
            int bracketsCounter = 0;
            int operandsCounter = 0;

            for (int i = startIndex; i <= endIndex;)
            {
                currentSymbol = expression[i];

                if (char.IsDigit(currentSymbol))
                {
                    ++operandsCounter;

                    while (++i <= endIndex && char.IsDigit(expression[i])) ;

                    if (i > endIndex)
                        break;

                    if (expression[i] == decimalSeparator)
                    {
                        if (i == endIndex || !char.IsDigit(expression[i + 1]))
                            return new ErrorCode(ErrorCodeEnum.InvalidSeparator, "Розділювач дробових чисел поставлено неправильно", i);

                        while (++i <= endIndex && char.IsDigit(expression[i])) ;

                        if (i > endIndex)
                            break;
                    }

                    currentSymbol = expression[i];

                    if (currentSymbol == decimalSeparator)
                        return new ErrorCode(ErrorCodeEnum.InvalidSeparator, "Два розділювачі дробових чисел", i);

                    if (currentSymbol == '(' || char.IsLetter(currentSymbol))
                        --operandsCounter;

                    continue;
                }

                if (IsBinOperator(currentSymbol))
                {
                    if (operandsCounter == 0)
                    {
                        if (currentSymbol == '+' || currentSymbol == '-')
                            if (i < endIndex)
                                if (expression[++i] == '(' || char.IsLetterOrDigit(expression[i]))
                                    continue;

                        return new ErrorCode(ErrorCodeEnum.NoOperand, $"Перед оператором \"{currentSymbol}\" пропущено операнд", i);
                    }

                    while (++i < endIndex && expression[i] == ' ') ;

                    if (i <= endIndex)
                        currentSymbol = expression[i];

                    if (i > endIndex || currentSymbol == ')' || IsBinOperator(currentSymbol))
                        return new ErrorCode(ErrorCodeEnum.NoOperand, "Пропущено операнд", i);

                    --operandsCounter;
                    continue;
                }

                if (IsPostfixOperator(currentSymbol))
                {
                    if (operandsCounter < 1)
                        return new ErrorCode(ErrorCodeEnum.NoOperand, $"Перед оператором \"{currentSymbol}\" пропущено операнд", i);

                    if (++i <= endIndex)
                    {
                        currentSymbol = expression[i];

                        if (currentSymbol == '(' || char.IsLetterOrDigit(currentSymbol))
                            --operandsCounter;
                    }

                    continue;
                }

                if (char.IsLetter(currentSymbol))
                {
                    int j = i;

                    while (++i <= endIndex && char.IsLetter(expression[i])) ;

                    substr = expression.Substring(j, i - j);

                    if (constants.IsConstant(substr))
                    {
                        if (i <= endIndex)
                            currentSymbol = expression[i];

                        if (currentSymbol != '(' && !char.IsDigit(currentSymbol))
                            ++operandsCounter;

                        continue;
                    }

                    if (i > endIndex || expression[i] != '(')
                        return new ErrorCode(ErrorCodeEnum.NoConstant, $"Константа \"{substr}\" не підтримується", j);

                    if (!IsFunction(substr))
                        return new ErrorCode(ErrorCodeEnum.NoFunction, $"Функція з назвою \"{substr}\" не підтримується", j);

                    continue;
                }

                switch (currentSymbol)
                {
                    case '(':
                        while (++i < endIndex && expression[i] == ' ') ;

                        if (i <= endIndex && expression[i] == ')')
                            return new ErrorCode(ErrorCodeEnum.EmptyBrackets, "Пусті дужки", i);

                        ++bracketsCounter;
                        continue;

                    case ')':
                        if (--bracketsCounter < 0)
                            return new ErrorCode(ErrorCodeEnum.NoOpeningBracket, "Пропущено відкриваючу дужку перед закриваючою", i);

                        if (++i <= endIndex)
                        {
                            currentSymbol = expression[i];

                            if (currentSymbol == '(' || char.IsLetterOrDigit(currentSymbol))
                                --operandsCounter;
                        }

                        continue;

                    case ' ':
                        ++i;
                        continue;

                    default:
                        return new ErrorCode(ErrorCodeEnum.InvalidData, $"Символ \"{currentSymbol}\" не є частиною математичного виразу", i);
                }
            }

            if (bracketsCounter != 0)
                return new ErrorCode(ErrorCodeEnum.NoClosingBracket, "Пропущено закриваючу дужку", endIndex);

            if (operandsCounter != 1)
                return new ErrorCode(ErrorCodeEnum.NoOperator, "Пропущено оператор між операндами");

            return new ErrorCode();
        }

        public static bool IsCorrectNumber(double number)
        {
            if (double.IsNaN(number) || double.IsInfinity(number))
                return false;

            return true;
        }
    }
}
