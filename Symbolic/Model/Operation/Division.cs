using System;
using Symbolic.Model.Base;
using Symbolic.Model.Template;

namespace Symbolic.Model.Operation
{
    public class Division : Operator
    {
        private Division(Function a, Function b) : base(a, b) { }

        public static Function New(Function a, Function b)
        {
            if (b is Constant && Math.Abs((b.Calc(0)) - 1) <= 10e-6)
                return a;
            if (a is Constant && Math.Abs((a.Calc(0))) <= 10e-6)
                return Funcs.Zero;
            if (b is Constant && Math.Abs((b.Calc(0))) <= 10e-6)
                throw new DivideByZeroException("Function b cannot be zero constant");

            if (b is Constant)
                return (1 / b.Calc(0)) * a;

            return new Division(a, b);
        }

        /// <summary>
        /// Calculate function
        /// </summary>
        /// <param name="val"> Argument value </param>
        /// <returns> Function value </returns>
        public override double Calc(double val)
        {
            return LeftFunc.Calc(val) / RightFunc.Calc(val);
        }

        /// <summary>
        /// Derivative rule
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return (LeftFunc.Derivative() * RightFunc - RightFunc.Derivative() * LeftFunc) / (RightFunc * RightFunc);
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return  LeftFunc + "/ (" + RightFunc + ")";
        }

        /// <summary>
        /// Latex view
        /// </summary>
        /// <returns></returns>
        public override string ToLatexString()
        {
            return @"\frac{" + LeftFunc.ToLatexString() + "}{" + RightFunc.ToLatexString() + "}";
        }

        #endregion
    }
}
