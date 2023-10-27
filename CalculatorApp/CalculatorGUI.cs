using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class CalculatorGUI : Form
    {
        private MathValidator validator;
        private History history;

        public CalculatorGUI()
        {
            validator = new MathValidator();
            history = new History();

            InitializeComponent();

            expressionTBox.KeyPress += ExpressionTBox_KeyPress;

            foreach (Button button in buttons)
                button.Click += Button_Click;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button) sender;
            string exprText = expressionTBox.Text;
            int exprLen = exprText.Length;
            string btnText = clickedButton.Text;
            char btnFirstSymbol = btnText[0];

            if (char.IsDigit(btnFirstSymbol))
            {
                expressionTBox.AppendText(btnText);

                return;
            }

            if (exprLen > 0 && validator.IsDecimalSeparator(exprText.Last()))
                return;

            if (validator.IsDecimalSeparator(btnFirstSymbol))
            {
                if (exprLen == 0 || !char.IsDigit(exprText.Last()))
                    return;

                expressionTBox.AppendText(btnText);

                return;
            }

            if (validator.IsBinOperator(btnFirstSymbol))
            {
                if (exprLen == 0 && (btnFirstSymbol != '-' && btnFirstSymbol != '+'))
                    return;

                if (exprLen > 0)
                {
                    char exprLastSymbol = exprText.Last();

                    if (exprLastSymbol == '(' && (btnFirstSymbol != '-' && btnFirstSymbol != '+'))
                        return;

                    if (validator.IsBinOperator(exprLastSymbol))
                        return;
                }

                expressionTBox.AppendText(btnText);

                return;
            }

            if (validator.IsPostfixOperator(btnFirstSymbol))
            {
                if (exprLen == 0)
                    return;

                char previousSymbol = exprText.Last();

                if (char.IsLetterOrDigit(previousSymbol) || previousSymbol == ')' || validator.IsPostfixOperator(previousSymbol))
                    expressionTBox.AppendText(btnText);

                return;
            }

            if (validator.IsConstant(btnText))
            {
                if (exprLen > 0 && char.IsLetter(exprText.Last()))
                    return;

                expressionTBox.AppendText(btnText);

                return;
            }

            if (validator.IsFunction(btnText))
            {
                expressionTBox.AppendText($"{btnText}(");

                return;
            }

            if (validator.IsBracket(btnFirstSymbol))
            {
                expressionTBox.AppendText(btnText);

                return;
            }

            if (btnFirstSymbol == '⌫')
            {
                if (exprLen == 0)
                    return;

                expressionTBox.Text = exprText.Remove(exprLen - 1, 1);

                return;
            }
            
            if (btnFirstSymbol == '√')
            {
                expressionTBox.AppendText("sqrt(");

                return;
            }

            if (btnFirstSymbol == '=')
            {
                PerformCalculation();

                return;
            }

            if (btnText == "CE")
            {
                expressionTBox.Clear();

                return;
            }
        }
        private void ExpressionTBox_KeyPress(object sender, KeyPressEventArgs key)
        {
            switch (key.KeyChar)
            {
                case (char) Keys.Enter:
                    key.Handled = true; 

                    PerformCalculation();
                    return;

                case (char) Keys.Escape:
                    key.Handled = true;

                    expressionTBox.Clear();
                    return;
            }
        }

        private void PerformCalculation()
        {
            string expression = expressionTBox.Text;
            string resultStr;
            double result;

            try
            {
                result = Calculator.Calculate(validator, expression, 0, expression.Length - 1);
                resultStr = MathConverter.ToReadable(result);

                history.AddRecord($"{expression} = {resultStr}");

                resultLabel.Text = resultStr;

                historyTBox.AppendText(history.LastRecord());
                historyTBox.AppendText("\r\n\r\n");
            }
            catch (Exception error)
            {
                resultLabel.Text = error.Message;
            }
        }
    }
}
