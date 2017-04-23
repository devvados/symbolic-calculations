using System;
using Symbolic.Model.Base;
using Symbolic.Model.Template;

namespace Symbolic.Model.Operation
{
    public class Addition : Operator
    {
        private Addition(Function a, Function b) : base(a, b) { }

        public static Function New(Function a, Function b)
        {
            if (b is Constant && Math.Abs((b.Calc(0))) <= 10e-6)
                return a;
            if (a is Constant && Math.Abs((a.Calc(0))) <= 10e-6)
                return b;
            if (a is Constant && b is Constant && Math.Abs(a.Calc(0) + b.Calc(0)) <= 10e-6)
                return Funcs.Zero;
            if (a is Constant && b is Constant)
                return new Constant(a.Calc(0) + b.Calc(0));

            return new Addition(a, b);
        }

        public override double Calc(double val)
        {
            return LeftFunc.Calc(val) + RightFunc.Calc(val);
        }

        /// <summary>
        /// Derivative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return LeftFunc.Derivative() + RightFunc.Derivative();
        }

        public override string ToString()
        {
            return LeftFunc + " + (" + RightFunc + ")";
        }
    }
}
