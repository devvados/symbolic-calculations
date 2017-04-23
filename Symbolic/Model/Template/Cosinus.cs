using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template
{
    public class Cosinus : Function
    {
        public Function innerF;

        public Cosinus()
        {

        }
        public Cosinus(Function f)
        {
            innerF = f;
        }
        public override double Calc(double val)
        {
            return Math.Cos(val);
        }

        /// <summary>
        /// Deirvative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return (innerF != null) ? -1 * Funcs.Sin(innerF) * innerF.Derivative() : -1 * Funcs.Sin();
        }

        public override string ToString()
        {
            return (innerF != null) ? $"cos({innerF.ToString()})" : "cos(x)";
        }
    }
}
