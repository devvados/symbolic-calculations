using Symbolic.Model.Base;
using Symbolic.Model.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return leftFunc.Calc(rightFunc.Calc(val));
        }

        /// <summary>
        /// Derivative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return (leftFunc[1] % rightFunc) * rightFunc[1];
        }

        public override string ToString()
        {
            return "(" + leftFunc + " ◦ " + rightFunc + ")";
        }
    }
}
