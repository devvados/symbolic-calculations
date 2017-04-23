using Symbolic.Model.Base;
using Symbolic.Model.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override double Calc(double val)
        {
            return leftFunc.Calc(val) / rightFunc.Calc(val);
        }

        /// <summary>
        /// Derivative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return (leftFunc.Derivative() * rightFunc - rightFunc.Derivative() * leftFunc) / (rightFunc * rightFunc);
        }

        public override string ToString()
        {
            return leftFunc + " / (" + rightFunc + ")";
        }
    }
}
