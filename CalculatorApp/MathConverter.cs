using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorApp
{
    public static class MathConverter
    {
        private static byte Priority(char oper)
        {
            switch (oper)
            {
                case '+':
                case '-':
                    return 1;

                case '*':
                case '/':
                    return 2;

                case '(':
                    return 0;

                default:
                    return 3;
            }
        }
        private static AssociativityEnum OperatorAssociativity(char oper)
        {
            switch (oper)
            {
                case 'n':                                // function "neg"[0] -> 'n' (unary negation)
                case '^':
                    return AssociativityEnum.Right;

                default:
                    return AssociativityEnum.Left;
            }
        }
        private static void AddOperator(StringBuilder postfixForm, Stack<string> stack, char oper, ref int operandsCounter)
        {
            char previosElement;
            byte prevElPriority;
            byte currentPriority = Priority(oper);

            while (stack.Count > 0)
            {
                previosElement = stack.Peek()[0];
                prevElPriority = Priority(previosElement);

                if (prevElPriority > currentPriority
                    || prevElPriority == currentPriority && OperatorAssociativity(previosElement) == AssociativityEnum.Left)
                {
                    postfixForm.Append(stack.Pop());
                    postfixForm.Append(' ');

                    continue;
                }

                break;
            }

            stack.Push(oper.ToString());

            --operandsCounter;
        }

        public static string InfixToPostfix(MathValidator validator, string expression, int startIndex, int endIndex)
        {
            ErrorCode checkExpr = validator.CheckExpression(expression, startIndex, endIndex);

            if (checkExpr.Code != ErrorCodeEnum.NoErrors)
                throw checkExpr;

            StringBuilder postfixForm = new StringBuilder(endIndex - startIndex + 1);
            Stack<string> stack = new Stack<string>(postfixForm.Capacity / 5);
            char decimalSeparator = validator.DecimalSeparator;
            char currentSymbol;
            int operandsCounter = 0;
            string substr;

            for (int i = startIndex, j; i <= endIndex;)
            {
                currentSymbol = expression[i];

                if (char.IsDigit(currentSymbol))
                {
                    ++operandsCounter;

                    j = i;

                    while (++i <= endIndex && char.IsDigit(expression[i])) ;

                    if (i <= endIndex && expression[i] == decimalSeparator)
                        while (++i <= endIndex && char.IsDigit(expression[i])) ;

                    postfixForm.Append(expression, j, i - j);
                    postfixForm.Append(' ');

                    if (i <= endIndex)
                        currentSymbol = expression[i];

                    if (currentSymbol == '(' || char.IsLetter(currentSymbol))
                        AddOperator(postfixForm, stack, '*', ref operandsCounter);
                }

                if (validator.IsBinOperator(currentSymbol))
                {
                    if (operandsCounter == 0)
                    {
                        if (currentSymbol == '-')
                            stack.Push("neg");

                        ++i;
                        continue;
                    }

                    AddOperator(postfixForm, stack, currentSymbol, ref operandsCounter);

                    ++i;
                    continue;
                }

                if (validator.IsPostfixOperator(currentSymbol))
                {
                    postfixForm.Append(currentSymbol);
                    postfixForm.Append(' ');

                    if (++i <= endIndex)
                    {
                        currentSymbol = expression[i];

                        if (currentSymbol == '(' || char.IsLetterOrDigit(currentSymbol))
                            AddOperator(postfixForm, stack, '*', ref operandsCounter);
                    }

                    continue;
                }

                if (char.IsLetter(currentSymbol))
                {
                    j = i;

                    while (++i <= endIndex && char.IsLetter(expression[i])) ;

                    if (i <= endIndex)
                        currentSymbol = expression[i];

                    substr = expression.Substring(j, i - j);

                    if (validator.IsConstant(substr))
                    {
                        ++operandsCounter;

                        postfixForm.Append(substr);
                        postfixForm.Append(' ');

                        if (currentSymbol == '(' || char.IsDigit(currentSymbol))
                            AddOperator(postfixForm, stack, '*', ref operandsCounter);
                    }
                    else
                        stack.Push(substr);
                }

                switch (currentSymbol)
                {
                    case '(':
                        stack.Push("(");

                        ++i;
                        continue;

                    case ')':
                        while (stack.Peek() != "(")
                        {
                            postfixForm.Append(stack.Pop());
                            postfixForm.Append(' ');
                        }

                        stack.Pop();

                        if (stack.Count > 0 && char.IsLetter(stack.Peek()[0]))
                        {
                            postfixForm.Append(stack.Pop());
                            postfixForm.Append(' ');
                        }

                        if (++i <= endIndex)
                        {
                            currentSymbol = expression[i];

                            if (currentSymbol == '(' || char.IsLetterOrDigit(currentSymbol))
                                AddOperator(postfixForm, stack, '*', ref operandsCounter);
                        }

                        continue;

                    case ' ':
                        ++i;
                        continue;
                }
            }

            while (stack.Count > 0)
            {
                postfixForm.Append(stack.Pop());
                postfixForm.Append(' ');
            }

            postfixForm.Remove(postfixForm.Length - 1, 1);

            return postfixForm.ToString();
        }

        public static string ToReadable(double number)      // Повертає number у вигляді стрічки, яка легко читається
        {
            double numberAbs = Math.Abs(number);

            if (numberAbs < 1E-9)
                return "0";

            if (numberAbs > 1E12)
                return $"{number:E12}";

            if (number == Math.Truncate(numberAbs))
                return $"{number:N0}";

            return $"{number:0.#########}"; ;
        }
    }
}
