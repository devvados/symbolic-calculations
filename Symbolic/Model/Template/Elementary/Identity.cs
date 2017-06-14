using System;
using Symbolic.Model.Base;

namespace Symbolic.Model.Template
{
    /// <summary>
    /// Linear function
    /// </summary>
    public class Identity : Function
    {
        string symbol;

        //public Identity(string s)
        //{
        //    Symbol = s;
        //}

        public string Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                symbol = value;
            }
        }

        /// <summary>
        /// Calculate function
        /// </summary>
        /// <param name="val"> Argument value </param>
        /// <returns> Function value </returns>
        public override double Calc(double val)
        {
            return val;
        }

        /// <summary>
        /// Deirvative rule
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return Funcs.Zero + 1;
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"x";
        }

        /// <summary>
        /// Latex view
        /// </summary>
        /// <returns></returns>
        public override string ToLatexString()
        {
            return $"x";
        }

        #endregion
    }
}
