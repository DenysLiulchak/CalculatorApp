using System;

namespace CalculatorApp
{
    public class MathConstants
    {
        private readonly MathConstant[] constants = new MathConstant[]
        {
            new MathConstant("π", Math.PI),
            new MathConstant("pi", Math.PI),
            new MathConstant("e", Math.E)
        };

        public bool IsConstant(string str)
        {
            int length = constants.Length;

            for (int i = 0; i < length; ++i)
                if (constants[i].Name == str)
                    return true;

            return false;
        }
        public double GetValue(string constantName)
        {
            int length = constants.Length;

            for (int i = 0; i < length; ++i)
                if (constants[i].Name == constantName)
                    return constants[i].Value;

            throw new Exception($"Константа \"{constantName}\" не підтримується");
        }

        public MathConstants() { }
        public MathConstants(MathConstant[] constants)
        {
            int length = constants.Length;
            this.constants = new MathConstant[length];

            for (int i = 0; i < length; ++i)
                this.constants[i] = constants[i];
        }
        public MathConstants(MathConstants other)
        {
            int length = other.constants.Length;
            this.constants = new MathConstant[length];

            for (int i = 0; i < length; ++i)
                this.constants[i] = other.constants[i];
        }
    }
}
