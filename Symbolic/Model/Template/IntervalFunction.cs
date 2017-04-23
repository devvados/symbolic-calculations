using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template
{
    /// <summary>
    /// Interval function returns 1 if x is from [a,b] interval, and 0 otherwise.
    /// </summary>
    public class IntervalFunction : Function
    {
        public IntervalFunction(double a, double b)
        {
            this.a = a;
            this.b = b;
        }

        public override double Calc(double val)
        {
            if (a <= val && val <= b)
                return 1;
            return 0;
        }

        /// <summary>
        /// Deirvative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return Funcs.Zero;
        }

        public override string ToString()
        {
            return $"IntervalFunction[{a}, {b}](x)";
        }

        private double a;
        private double b;
    }
}
