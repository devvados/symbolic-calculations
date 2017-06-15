using System;
using Symbolic.Model.Base;
using Symbolic.Model.Template;
using static Symbolic.Model.Template.Funcs;

namespace Symbolic.Model.Operation
{
    public class Power : Operator
    {
        public Power(Function a, Function b) : base(a, b) { }

        public static Function New(Function a, Function b)
        {
            if (b is Constant && Math.Abs(b.Calc(0)) < 0.0001)
                return new Constant(1);
            return new Power(a, b);
        }

        /// <summary>
        /// Calculate function
        /// </summary>
        /// <param name="val"> Argument value </param>
        /// <returns> Function value </returns>
        public override double Calc(double val)
        {
            return Math.Pow(LeftFunc.Calc(val), RightFunc.Calc(val));
        }

        /// <summary>
        /// Derivative rule
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            if (RightFunc is Constant)
            {
                //x^n dx = n*x^(n-1)
                return RightFunc * (LeftFunc ^ (RightFunc - new Constant(1))) * LeftFunc.Derivative();
            }
            else
            {
                //u^v dx = (v'*ln(u)*(u'*v)/u)*u^v
                return (RightFunc.Derivative() * Ln(LeftFunc) + ((LeftFunc.Derivative() * RightFunc) / LeftFunc)) * this;
            }
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + LeftFunc + ")" + "^(" + RightFunc + ")";
        }

        /// <summary>
        /// Latex view
        /// </summary>
        /// <returns></returns>
        public override string ToLatexString()
        {
            return LeftFunc.ToLatexString() + "^{" + RightFunc.ToLatexString() + "}";
        }

        #endregion
    }
}
