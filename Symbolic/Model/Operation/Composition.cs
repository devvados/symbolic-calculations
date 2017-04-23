using System;
using Symbolic.Model.Base;
using Symbolic.Model.Template;

namespace Symbolic.Model.Operation
{
    public class Composition : Operator
    {
        /// <summary>
        /// f(x) = a(b(x)), f(x) = (a . b) (x)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private Composition(Function a, Function b) : base(a, b) { }

        public static Function New(Function a, Function b)
        {
            if (b is Identity)
                return a;
            if (a is Identity)
                return b;
            if (a is Constant)
                return a;
            if (b is Constant)
                return b;

            return new Composition(a, b);
        }

        public override double Calc(double val)
        {
            return LeftFunc.Calc(RightFunc.Calc(val));
        }

        /// <summary>
        /// Derivative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return (LeftFunc[1] % RightFunc) * RightFunc[1];
        }

        public override string ToString()
        {
            return "(" + LeftFunc + " ◦ " + RightFunc + ")";
        }
    }
}
