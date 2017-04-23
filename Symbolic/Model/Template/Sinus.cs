using Symbolic.Model.Base;
using System;

namespace Symbolic.Model.Template
{
    public class Sinus : Function
    {
        private readonly Function _innerF;

        public Sinus()
        {

        }

        public Sinus(Function f)
        {
            _innerF = f;
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
            if (_innerF != null)
                return Funcs.Cos(_innerF) * _innerF.Derivative();
            else
                return Funcs.Cos();
        }

        public override string ToString()
        {
            return _innerF != null ? $"sin({_innerF})" : "sin(x)";
        }
    }
}
