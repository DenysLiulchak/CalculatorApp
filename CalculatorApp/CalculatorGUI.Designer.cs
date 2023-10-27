using CalculatorApp.Properties;
using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;

namespace CalculatorApp
{
    partial class CalculatorGUI
    {
        private TableLayoutPanel tableLayoutPanel;
        private Button[] buttons;
        private TextBox expressionTBox;
        private TextBox historyTBox;
        private Label resultLabel;
        private Label historyLabel;
        private const int BUTTONS_COUNT = 42;

        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculatorGUI));

            CreateElements();
            tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();

            TableLayoutPanelSettings();
            AddElementsToPanel();
            ButtonsSettings();
            ExpressionTBoxSettings();
            HistoryTBoxSettings();
            ResultLabelSettings();
            HistoryLabelSettings();
            FormSettings(resources);

            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        private void CreateElements()
        {
            tableLayoutPanel = new TableLayoutPanel();

            buttons = new Button[BUTTONS_COUNT];

            for (int i = 0; i < BUTTONS_COUNT; ++i)
                buttons[i] = new Button();

            expressionTBox = new TextBox();
            historyTBox = new TextBox();
            resultLabel = new Label();
            historyLabel = new Label();
        }

        private void AddElementsToPanel()
        {
            tableLayoutPanel.Controls.Add(expressionTBox, 0, 0);
            tableLayoutPanel.Controls.Add(resultLabel, 0, 1);
            tableLayoutPanel.Controls.Add(historyLabel, 6, 0);
            tableLayoutPanel.Controls.Add(historyTBox, 6, 1);

            const int FIRST_ROW = 2;
            const int FIRST_COLUMN = 0;
            const int LAST_ROW = 8;
            const int LAST_COLUMN = 5;

            IEnumerator iter = buttons.GetEnumerator();
            iter.MoveNext();

            for (int i = FIRST_ROW; i <= LAST_ROW; ++i)
                for (int j = FIRST_COLUMN; j <= LAST_COLUMN; ++j, iter.MoveNext())
                    tableLayoutPanel.Controls.Add((Button)iter.Current, j, i);
        }

        private void TableLayoutPanelSettings()
        {
            ColumnStyle columnStyle;
            RowStyle rowStyle =  new RowStyle(System.Windows.Forms.SizeType.Percent, 10F);

            tableLayoutPanel.BackColor = System.Drawing.Color.FromArgb(15, 26, 25);
            tableLayoutPanel.ColumnCount = 7;
            for (int i = 1; i <= 6; ++i)
            {
                columnStyle = new ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.6666667F);
                tableLayoutPanel.ColumnStyles.Add(columnStyle);
            }
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 380F));

            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.Padding = new System.Windows.Forms.Padding(0, 10, 0, 5);
            tableLayoutPanel.RowCount = 9;
            tableLayoutPanel.RowStyles.Add(rowStyle);
            tableLayoutPanel.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            for (int i = 3; i <= 9; ++i)
            {
                rowStyle = new RowStyle(System.Windows.Forms.SizeType.Percent, 10F);
                tableLayoutPanel.RowStyles.Add(rowStyle);
            }

            tableLayoutPanel.TabIndex = 0;
        }
        private void ButtonsSettings()
        {
            string[] buttonsText = new string[BUTTONS_COUNT]
            {
                "ln",     "log",      "π",    "e", "CE",                                  "⌫",
                "sin",    "cos",      "abs",  "√", "%",                                   "^",
                "arcsin", "arccos",   "(",    ")", "!",                                   "/",
                "tg",     "ctg",      "7",    "8", "9",                                   "*",
                "arctg",  "arcctg",   "4",    "5", "6",                                   "-",
                "sec",    "cosec",    "1",    "2", "3",                                   "+",
                "arcsec", "arccosec", "sign", "0", validator.DecimalSeparator.ToString(), "="
            };

            Color BackColor1 = System.Drawing.Color.FromArgb(72, 49, 77);
            Color ForeColor1 = System.Drawing.Color.Silver;
            Color BackColor2 = System.Drawing.Color.FromArgb(72, 49, 77);
            Color ForeColor2 = System.Drawing.Color.Silver;
            Color BackColor3 = System.Drawing.Color.FromArgb(42, 71, 69);
            Color ForeColor3 = System.Drawing.Color.White;
            Color BackColor4 = System.Drawing.Color.FromArgb(20, 33, 32);
            Color ForeColor4 = System.Drawing.Color.White;

            const int section1First         = (int) ButtonSectionEnum.Section1First;
            const int section1Last          = (int) ButtonSectionEnum.Section1Last;
            const int section1ColumnsCount  = (int) ButtonSectionEnum.Section1ColumnsCount;
            const int section2First         = (int) ButtonSectionEnum.Section2First;
            const int section2Last          = (int) ButtonSectionEnum.Section2Last;
            const int section2ColumnsCount  = (int) ButtonSectionEnum.Section2ColumnsCount;
            const int section3First         = (int) ButtonSectionEnum.Section3First;
            const int section3Last          = (int) ButtonSectionEnum.Section3Last;
            const int section3ColumnsCount  = (int) ButtonSectionEnum.Section3ColumnsCount;
            const int section4First         = (int) ButtonSectionEnum.Section4First;
            const int section4Last          = (int) ButtonSectionEnum.Section4Last;
            const int section4ColumnsCount  = (int) ButtonSectionEnum.Section4ColumnsCount;

            int columnCount = tableLayoutPanel.ColumnCount - 1;

            Button currentButton;

            for (int i = 0; i < BUTTONS_COUNT; ++i)
            {
                currentButton = buttons[i];

                currentButton.Cursor = System.Windows.Forms.Cursors.Hand;
                currentButton.Dock = System.Windows.Forms.DockStyle.Fill;
                currentButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
                currentButton.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                currentButton.UseVisualStyleBackColor = false;
                currentButton.Text = buttonsText[i];
            }

            for (int i = section1First; i < section1Last; i += columnCount)
                for (int j = 0; j < section1ColumnsCount; ++j)
                {
                    currentButton = buttons[i + j];

                    currentButton.BackColor = BackColor1;
                    currentButton.ForeColor = ForeColor1;
                }

            for (int i = section2First; i < section2Last; i += columnCount)
                for (int j = 0; j < section2ColumnsCount; ++j)
                {
                    currentButton = buttons[i + j];

                    currentButton.BackColor = BackColor2;
                    currentButton.ForeColor = ForeColor2;
                }

            for (int i = section3First; i < section3Last; i += columnCount)
                for (int j = 0; j < section3ColumnsCount; ++j)
                {
                    currentButton = buttons[i + j];

                    currentButton.BackColor = BackColor3;
                    currentButton.ForeColor = ForeColor3;
                }

            for (int i = section4First; i < section4Last; i += columnCount)
                for (int j = 0; j < section4ColumnsCount; ++j)
                {
                    currentButton = buttons[i + j];

                    currentButton.BackColor = BackColor4;
                    currentButton.ForeColor = ForeColor4;
                }

            buttons[BUTTONS_COUNT - 1].BackColor = System.Drawing.Color.FromArgb(17, 0, 103);
            buttons[BUTTONS_COUNT - 1].ForeColor = ForeColor2;
        }
        private void ExpressionTBoxSettings()
        {
            expressionTBox.BackColor = System.Drawing.Color.FromArgb(12, 21, 20);
            tableLayoutPanel.SetColumnSpan(expressionTBox, 6);
            expressionTBox.Dock = System.Windows.Forms.DockStyle.Fill;
            expressionTBox.Font = new System.Drawing.Font("Arial", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            expressionTBox.ForeColor = System.Drawing.Color.RosyBrown;
            expressionTBox.Name = "expressionTBox";
            expressionTBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        }
        private void HistoryTBoxSettings()
        {
            historyTBox.BackColor = System.Drawing.Color.FromArgb(12, 21, 20);
            historyTBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            historyTBox.Dock = System.Windows.Forms.DockStyle.Fill;
            historyTBox.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            historyTBox.ForeColor = System.Drawing.Color.Silver;
            historyTBox.Multiline = true;
            historyTBox.Name = "historyTBox";
            historyTBox.ReadOnly = true;
            tableLayoutPanel.SetRowSpan(historyTBox, 8);
            historyTBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        }
        private void ResultLabelSettings()
        {
            resultLabel.BackColor = System.Drawing.Color.FromArgb(12, 21, 20);
            tableLayoutPanel.SetColumnSpan(resultLabel, 6);
            resultLabel.Cursor = System.Windows.Forms.Cursors.Default;
            resultLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            resultLabel.Font = new System.Drawing.Font("Arial", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            resultLabel.ForeColor = System.Drawing.Color.RosyBrown;
            resultLabel.Name = "resultLabel";
            resultLabel.Text = "Введіть вираз ↑";
            resultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        }
        private void HistoryLabelSettings()
        {
            historyLabel.BackColor = System.Drawing.Color.FromArgb(12, 21, 20);
            historyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            historyLabel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            historyLabel.ForeColor = System.Drawing.Color.RosyBrown;
            historyLabel.Name = "historyLabel";
            historyLabel.Text = "Історія";
            historyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        }
        private void FormSettings(System.ComponentModel.ComponentResourceManager resources)
        {
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(15, 26, 25);
            ClientSize = new System.Drawing.Size(1262, 673);
            Controls.Add(tableLayoutPanel);
            ForeColor = System.Drawing.Color.White;
            Icon = ((System.Drawing.Icon)(resources.GetObject("Calculator.ico")));
            MinimumSize = new System.Drawing.Size(1280, 720);
            Name = "CalculatorGUI";
            Text = "Калькулятор";
        }
    }
}