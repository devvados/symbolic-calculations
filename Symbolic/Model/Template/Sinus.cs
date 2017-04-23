using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template
{
    public class Sinus : Function
    {
        public Function innerF;

        public Sinus()
        {

        }

        public Sinus(Function f)
        {
            innerF = f;
        }

        public override double Calc(double val)
        {
            return Math.Sin(val);
        }

        /// <summary>
        /// Deirvative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            if (innerF != null)
                return Funcs.Cos(innerF) * innerF.Derivative();
            else
                return Funcs.Cos();
        }

        public override string ToString()
        {
            if (innerF != null)
                return $"sin({innerF.ToString()})";
            else
                return "sin(x)";
        }
    }
}
