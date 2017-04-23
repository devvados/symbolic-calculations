using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template
{
    /// <summary>
    /// Log (x) with base a
    /// </summary>
    public class Logarithm : Function
    {
        private double a;
        private Function innerF;
        public Logarithm(double a = Math.E)
        {
            this.a = a;
        }

        public Logarithm(Function f, double a = Math.E)
        {
            this.a = a;
            innerF = f;
        }

        public override double Calc(double val)
        {
            return Math.Log(val, a);
        }

        /// <summary>
        /// Deirvative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            if (innerF != null)
            {
                if (Math.Abs(a - Math.E) <= 10e-6)
                    return 1 / (innerF) * innerF.Derivative();
                else
                    return 1 / (Funcs.Id * /*Funcs.Ln(new Constant(a))*/ Math.Log(a, Math.E)) * innerF.Derivative();
            }
            else
                return 1 / (Funcs.Id * /*Funcs.Ln(new Constant(a)*/ Math.Log(a, Math.E));
        }

        public override string ToString()
        {
            if (Math.Abs(a - Math.E) <= 10e-6)
                return (innerF != null) ? $"ln({innerF.ToString()})" : "ln(x)";
            else
                return (innerF != null) ? $"log[{a}]({innerF.ToString()})" : "log[" + a + "](x)"; ;
        }
    }
}
