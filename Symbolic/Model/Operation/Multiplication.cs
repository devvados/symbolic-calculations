using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override double Calc(double val)
        {
            var a = leftFunc.Calc(val);
            if (Math.Abs(a) <= 10e-6)
                return a;
            var b = rightFunc.Calc(val);
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
            return leftFunc.Derivative() * rightFunc + leftFunc * rightFunc.Derivative();
        }

        public override string ToString()
        {
            return leftFunc + " * (" + rightFunc + ")";
        }
    }
}
