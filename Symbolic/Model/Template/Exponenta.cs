using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template
{
    public class Exponenta : Function
    {
        private double a;
        private Function innerF;
        public Exponenta(double a = Math.E)
        {
            this.a = a;
        }

        public Exponenta(Function f, double a = Math.E)
        {
            this.a = a;
            innerF = f;
        }

        public override double Calc(double val)
        {
            return Math.Pow(a, val);
        }

        /// <summary>
        /// Deirvative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            if (innerF != null)
                return new Exponenta(innerF) * innerF.Derivative();
            else
                return this; //new Constant(Math.Log(a, Math.E)) * this
        }

        public override string ToString()
        {
            if (innerF != null)
                return $"e^({innerF.ToString()})";
            else
            {
                if (Math.Abs(a - Math.E) <= 10e-6)
                    return "e^x";
                return $"{a}^x";
            }
        }
    }
}
