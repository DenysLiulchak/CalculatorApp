using System;

namespace CalculatorApp
{
    public class ErrorCode : Exception
    {
        private readonly ErrorCodeEnum errorCode;
        private readonly int index;

        public ErrorCodeEnum Code
        {
            get
            {
                return errorCode;
            }
        }
        public int Index
        {
            get
            {
                return index;
            }
        }

        public ErrorCode(ErrorCodeEnum errorCode = ErrorCodeEnum.NoErrors, string message = "Немає помилок", int index = -1) : base(message)
        {
            this.errorCode = errorCode;
            this.index = index;
        }
    }
}
