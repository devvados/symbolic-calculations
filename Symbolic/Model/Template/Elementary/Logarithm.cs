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

        /// <summary>
        /// Ln(x) or Log[a](x)
        /// </summary>
        /// <param name="a"></param>
        public Logarithm(double a = Math.E)
        {
            _a = a;
        }

        /// <summary>
        /// Ln(f(x)) or Log[a](f(x))
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        public Logarithm(Function f, double a = Math.E)
        {
            _a = a;
            _innerF = f;
        }

        /// <summary>
        /// Calculate function
        /// </summary>
        /// <param name="val"> Argument value </param>
        /// <returns> Function value </returns>
        public override double Calc(double val)
        {
            return Math.Log(val, _a);
        }

        /// <summary>
        /// Deirvative rule
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
                return $"ln({_innerF})";
            else
                return $"log[{_a}]({_innerF})";
        }

        public override string ToLatexString()
        {
            if (Math.Abs(_a - Math.E) <= 10e-6)
                return $@"\ln ({_innerF.ToLatexString()})";
            else
                return $@"\log_{_a} ({_innerF.ToLatexString()})";
        }

        #endregion
    }
}
