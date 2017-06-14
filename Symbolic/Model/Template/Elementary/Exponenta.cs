using Symbolic.Model.Base;
using static Symbolic.Model.Template.Funcs;
using System;

namespace Symbolic.Model.Template
{
    /// <summary>
    /// Exponential function
    /// </summary>
    public class Exponenta : Function
    {
        private readonly double _a;
        private readonly Function _innerF;

        /// <summary>
        /// e^x or a^x
        /// </summary>
        /// <param name="a"></param>
        public Exponenta(double a = Math.E)
        {
            _a = a;
        }

        /// <summary>
        /// e^(f(x)) or a^(f(x))
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        public Exponenta(Function f, double a = Math.E)
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
            return Math.Pow(_a, val);
        }

        /// <summary>
        /// Deirvative rule
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            if (Math.Abs(_a - Math.E) <= 10e-6)
                return Exp(_innerF) * _innerF.Derivative();
            else
                return this * Ln(new Constant(_a));
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Math.Abs(_a - Math.E) <= 10e-6)
                return $"exp({_innerF})";
            else
                return $"{_a}^({_innerF})";
        }

        /// <summary>
        /// Latex view
        /// </summary>
        /// <returns></returns>
        public override string ToLatexString()
        {
            if (Math.Abs(_a - Math.E) <= 10e-6)
                return @"\exp \left(" + _innerF.ToLatexString() + @"\right)";
            else
                return new Constant(_a).ToLatexString() + @"^\left(" + _innerF.ToLatexString() + @"\right)";
        }

        #endregion
    }
}
