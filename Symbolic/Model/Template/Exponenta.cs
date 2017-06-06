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
            if (_innerF != null)
                return new Exponenta(_innerF) * _innerF.Derivative();
            else
                return this; 
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_innerF != null)
                return $"exp({_innerF})";
            else
            {
                return Math.Abs(_a - Math.E) <= 10e-6 ? "exp(x)" : $"{_a}^x";
            }
        }
        
        #endregion
    }
}
