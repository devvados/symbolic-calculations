using System;
using Symbolic.Model.Base;

namespace Symbolic.Model.Operation
{
    class Power : Operator
    {
        private Power(Function a, Function b) : base(a, b) { }

        public static Function New(Function a, Function b)
        {
            if (b is Constant && Math.Abs(b.Calc(0)) < 0.0001)
                return new Constant(1);
            return new Power(a, b);
        }
        public override double Calc(double val)
        {
            return Math.Pow(LeftFunc.Calc(val), RightFunc.Calc(val));
        }

        /// <summary>
        /// Derivative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return RightFunc * (LeftFunc ^ (RightFunc - new Constant(1))) * LeftFunc.Derivative();
        }

        public override string ToString()
        {
            return LeftFunc + "^(" + RightFunc + ")";
        }
    }
}
