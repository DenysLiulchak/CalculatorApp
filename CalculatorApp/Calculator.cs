using System;
using System.Collections.Generic;

namespace CalculatorApp
{
    public static class Calculator
    {
        private static double PerformUnOperation(Stack<double> operands, char operation)
        {
            double result = double.NaN;
            double operand = operands.Pop();

            switch (operation)
            {
                case '%':
                    result = operand / 100D;
                    break;

                case '!':
                    if (operand < 0D || operand != Math.Truncate(operand))
                        throw new ArgumentException("Не визначено: факторіл визначено лише для невід'ємних цілих чисел");

                    if (operand > 170D)
                        return double.PositiveInfinity;

                    result = 1D;

                    for (int i = 1; i <= operand; ++i)
                        result *= i;

                    break;
            }

            return result;
        }
        private static double PerformBinOperation(Stack<double> operands, char operation)
        {
            double result = double.NaN;
            double operand1;
            double operand2;

            switch (operation)
            {
                case '+':
                    result = operands.Pop() + operands.Pop();
                    break;

                case '-':
                    operand2 = operands.Pop();
                    operand1 = operands.Pop();

                    result = operand1 - operand2;

                    break;

                case '*':
                    result = operands.Pop() * operands.Pop();
                    break;

                case '/':
                    operand2 = operands.Pop();
                    operand1 = operands.Pop();

                    if (operand2 == 0D)
                        throw new DivideByZeroException("Не визначено: на нуль ділити не можна");

                    result = operand1 / operand2;

                    break;

                case '^':
                    operand2 = operands.Pop();
                    operand1 = operands.Pop();

                    if (operand1 < 0 && operand2 != Math.Floor(operand2))
                        throw new ArgumentException("Не визначено: від'ємну основу можна піднести лише до цілого степеня");

                    result = Math.Pow(operand1, operand2);

                    break;
            }

            return result;
        }
        private static double PerformFunction(Stack<double> operands, string functionName)
        {
            double result = double.NaN;
            double operand = operands.Pop();

            switch (functionName)
            {
                case "sqrt":
                    if (operand < 0D)
                        throw new ArgumentException("Не визначено: арифметичний корінь визначено тільки для невід'ємних чисел");

                    result = Math.Sqrt(operand);

                    break;

                case "sin":
                    result = Math.Sin(operand);

                    if (result > 1D || result < -1D)
                        throw new ArgumentException("Помилка");

                    break;

                case "cos":
                    result = Math.Cos(operand);

                    if (result > 1D || result < -1D)
                        throw new ArgumentException("Помилка");

                    break;

                case "tg":
                    result = Math.Tan(operand);
                    break;

                case "ctg":
                    result = 1D / Math.Tan(operand);
                    break;

                case "sec":
                    result = 1D / Math.Cos(operand);

                    if (result > -1D && result < 1D)
                        throw new ArgumentException("Помилка");

                    break;

                case "cosec":
                    result = 1D / Math.Sin(operand);

                    if (result > -1D && result < 1D)
                        throw new ArgumentException("Помилка");

                    break;

                case "ln":
                    if (operand <= 0D)
                        throw new ArgumentException("Не визначено: логарифм визначено тільки для додатніх чисел");

                    result = Math.Log(operand);

                    break;

                case "log":
                    if (operand <= 0D)
                        throw new ArgumentException("Не визначено: логарифм визначено тільки для додатніх чисел");

                    result = Math.Log10(operand);

                    break;

                case "abs":
                    result = Math.Abs(operand);
                    break;

                case "neg":
                    result = operand * -1D;
                    break;

                case "exp":
                    result = Math.Exp(operand);
                    break;

                case "arcsin":
                    if (operand > 1D || operand < -1D)
                        throw new ArgumentException("Не визначно: арксинус визначено на відрізку [-1, 1]");

                    result = Math.Asin(operand);

                    break;

                case "arccos":
                    if (operand > 1D || operand < -1D)
                        throw new ArgumentException("Не визначно: арктангенс визначено на відрізку [-1, 1]");

                    result = Math.Acos(operand);

                    break;

                case "arctg":
                    result = Math.Atan(operand);
                    break;

                case "arcctg":
                    result = Math.Atan(1D / operand);
                    break;

                case "arcsec":
                    result = Math.Acos(1D / operand);
                    break;

                case "arccosec":
                    result = Math.Asin(1D / operand);
                    break;

                case "sign":
                    result = Math.Sign(operand);
                    break;
            }

            return result;
        }

        public static double Calculate(MathValidator validator, string expression, int startIndex, int endIndex)
        {
            string postfixForm = MathConverter.InfixToPostfix(validator, expression, startIndex, endIndex);

            postfixForm += ' ';

            int length = postfixForm.Length;
            MathConstants constants = validator.Constants;
            Stack<double> operands = new Stack<double>(length / 3);
            string currentLexem;
            double currentResult;
            char previousSymbol;

            for (int i = 1, startIndxLexem = 0; i < length; startIndxLexem = ++i)
            {
                while (postfixForm[i] != ' ')
                    ++i;

                previousSymbol = postfixForm[i - 1];

                if (validator.IsPostfixOperator(previousSymbol))
                    currentResult = PerformUnOperation(operands, previousSymbol);

                else if (validator.IsBinOperator(previousSymbol))
                    currentResult = PerformBinOperation(operands, previousSymbol);

                else
                {
                    currentLexem = postfixForm.Substring(startIndxLexem, i - startIndxLexem);

                    if (char.IsDigit(previousSymbol))
                    {
                        operands.Push(double.Parse(currentLexem));

                        continue;
                    }

                    else if (constants.IsConstant(currentLexem))
                    {
                        operands.Push(constants.GetValue(currentLexem));

                        continue;
                    }

                    else
                        currentResult = PerformFunction(operands, currentLexem);
                }

                if (!MathValidator.IsCorrectNumber(currentResult))
                    throw new Exception("Помилка");

                operands.Push(currentResult);
            }

            return operands.Pop();
        }
    }
}
