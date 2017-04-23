using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template
{
    public class Identity : Function
    {
        public override double Calc(double val)
        {
            return val;
        }

        /// <summary>
        /// Deirvative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return Funcs.Zero + 1;
        }

        public override string ToString()
        {
            return "x";
        }
    }
}
