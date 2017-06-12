using Symbolic.Model.Base;
using System;

namespace Symbolic.Model.Template
{
    public class Exponenta : Function
    {
        private readonly double _a;
        private readonly Function _innerF;

        public Exponenta(double a = Math.E)
        {
            _a = a;
        }

        public Exponenta(Function f, double a = Math.E)
        {
            _a = a;
            _innerF = f;
        }

        public override double Calc(double val)
        {
            return Math.Pow(_a, val);
        }

        /// <summary>
        /// Deirvative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return new Exponenta(_innerF) * _innerF.Derivative();
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"exp({_innerF})";
        }

        public override string ToLatexString()
        {
            return $@"exp\left({_innerF.ToLatexString()}\right)";
        }

        #endregion
    }
}
