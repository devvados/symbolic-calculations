using Symbolic.Model.Base;
using System;

namespace Symbolic.Model.Template
{
    public class Cosinus : Function
    {
        private readonly Function _innerF;

        public Cosinus() { }

        public Cosinus(Function f)
        {
            _innerF = f;
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
            return (_innerF != null) ? -1 * Funcs.Sin(_innerF) * _innerF.Derivative() : (-1 * Funcs.Sin());
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (_innerF != null) ? $"cos({_innerF})" : "cos(x)";
        }

        #endregion
    }
}
