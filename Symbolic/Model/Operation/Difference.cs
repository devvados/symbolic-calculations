using Symbolic.Model.Base;
using Symbolic.Model.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Operation
{
    public class Difference : Operator
    {
        private Difference(Function a, Function b) : base(a, b) { }

        public static Function New(Function a, Function b)
        {
            if (b is Constant && Math.Abs((b.Calc(0))) <= 10e-6)
                return a;
            if (a is Constant && b is Constant && Math.Abs(a.Calc(0) - b.Calc(0)) <= 10e-6)
                return Funcs.Zero;
            if (a == b)
                return Funcs.Zero;
            if (a is Constant && b is Constant)
                return new Constant(a.Calc(0) - b.Calc(0));

            return new Difference(a, b);
        }

        public override double Calc(double val)
        {
            return leftFunc.Calc(val) - rightFunc.Calc(val);
        }

        /// <summary>
        /// Derivative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return leftFunc.Derivative() - rightFunc.Derivative();
        }

        public override string ToString()
        {
            return leftFunc + " - (" + rightFunc + ")";
        }
    }
}
