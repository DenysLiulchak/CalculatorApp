namespace CalculatorApp
{
    public enum ButtonIndexEnum : byte
    {
        Ln,
        Log,
        PI,
        E,
        CE,
        Back,
        Sin,
        Cos,
        Abs,
        Sqrt,
        Percent,
        Exponentiation,
        Arcsin,
        Arccos,
        OpeningBracket,
        ClosingBracket,
        Factorial,
        Division,
        Tg,
        Ctg,
        Digit7,
        Digit8,
        Digit9,
        Multiplication,
        Arctg,
        Arcctg,
        Digit4,
        Digit5,
        Digit6,
        Subtraction,
        Sec,
        Cosec,
        Digit1,
        Digit2,
        Digit3,
        Addition,
        Arcsec,
        Arccosec,
        Sign,
        Digit0,
        Separator,
        Equal
    }

    /// <summary>
    /// Індекси [перший, останній) елементів buttons та кількість стовпців для прямокутників із tableLayoutPanel з однаковим дизайном
    /// </summary>
    public enum ButtonSectionEnum : byte      
    {
        Section1First = 0,
        Section1Last = 6,
        Section1ColumnsCount = 6,

        Section2First = 11,
        Section2Last = 36,
        Section2ColumnsCount = 1,

        Section3First = 6,
        Section3Last = 38,
        Section3ColumnsCount = 2,

        Section4First = 8,
        Section4Last = 41,
        Section4ColumnsCount = 3,
    }
}