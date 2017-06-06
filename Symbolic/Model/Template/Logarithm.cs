using Symbolic.Model.Base;
using System;

namespace Symbolic.Model.Template
{
    /// <summary>
    /// Log (x) with base a
    /// </summary>
    public class Logarithm : Function
    {
        private readonly double _a;
        private readonly Function _innerF;

        public Logarithm(double a = Math.E)
        {
            _a = a;
        }

        public Logarithm(Function f, double a = Math.E)
        {
            _a = a;
            _innerF = f;
        }

        public override double Calc(double val)
        {
            return Math.Log(val, _a);
        }

        /// <summary>
        /// Deirvative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            if (_innerF != null)
            {
                if (Math.Abs(_a - Math.E) <= 10e-6)
                    return 1 / (_innerF) * _innerF.Derivative();
                else
                    return 1 / (Funcs.Id * Math.Log(_a, Math.E)) * _innerF.Derivative();
            }
            else
                return 1 / (Funcs.Id * Math.Log(_a, Math.E));
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Math.Abs(_a - Math.E) <= 10e-6)
                return (_innerF != null) ? $"ln({_innerF})" : "ln(x)";
            else
                return (_innerF != null) ? $"log[{_a}]({_innerF})" : "log[" + _a + "](x)";
        }

        #endregion
    }
}
