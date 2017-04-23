using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Operation
{
    class Power : Operator
    {
        private Power(Function a, Function b) : base(a, b) { }

        public static Function New(Function a, Function b)
        {
            if (b is Constant && b.Calc(0) == 0)
                return new Constant(1);
            return new Power(a, b);
        }
        public override double Calc(double val)
        {
            return Math.Pow(leftFunc.Calc(val), rightFunc.Calc(val));
        }

        /// <summary>
        /// Derivative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return rightFunc * (leftFunc ^ (rightFunc - new Constant(1))) * leftFunc.Derivative();
        }

        public override string ToString()
        {
            return leftFunc + "^(" + rightFunc + ")";
        }
    }
}
