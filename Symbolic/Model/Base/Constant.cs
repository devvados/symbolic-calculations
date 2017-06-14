using System;
using Symbolic.Model.Template;
using System.Globalization;

namespace Symbolic.Model.Base
{
    public class Constant : Function
    {
        private readonly double _value;

        public Constant(double val)
        {
            _value = val;
        }

        public override double Calc(double val)
        {
            return _value;
        }

        public override Function Derivative()
        {
            return Funcs.Zero;
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _value.ToString("0.####", CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// Latex view
        /// </summary>
        /// <returns></returns>
        public override string ToLatexString()
        {
            return $"{_value}";
        }

        #endregion
    }
}
