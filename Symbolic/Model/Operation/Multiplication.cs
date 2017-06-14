using System;
using Symbolic.Model.Base;

namespace Symbolic.Model.Operation
{
    public class Multiplication : Operator
    {
        private Multiplication(Function a, Function b) : base(a, b) { }

        public static Function New(Function a, Function b)
        {
            if (b is Constant && Math.Abs((b.Calc(0)) - 1) <= 10e-6)
                return a;
            if (a is Constant && Math.Abs((a.Calc(0)) - 1) <= 10e-6)
                return b;
            if (a is Constant && Math.Abs((a.Calc(0))) <= 10e-6)
                return new Constant(0);
            if (b is Constant && Math.Abs((b.Calc(0))) <= 10e-6)
                return new Constant(0);

            return new Multiplication(a, b);
        }

        /// <summary>
        /// Calculate function
        /// </summary>
        /// <param name="val"> Argument value </param>
        /// <returns> Function value </returns>
        public override double Calc(double val)
        {
            var a = LeftFunc.Calc(val);
            if (Math.Abs(a) <= 10e-6)
                return a;
            var b = RightFunc.Calc(val);
            if (Math.Abs(b) <= 10e-6)
                return b;
            return a * b;
        }

        /// <summary>
        /// Derivative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return LeftFunc.Derivative() * RightFunc + LeftFunc * RightFunc.Derivative();
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return LeftFunc + "*(" + RightFunc + ")";
        }

        /// <summary>
        /// Latex view
        /// </summary>
        /// <returns></returns>
        public override string ToLatexString()
        {
            return LeftFunc.ToLatexString() + @"\cdot " + RightFunc.ToLatexString();
        }

        #endregion
    }
}
