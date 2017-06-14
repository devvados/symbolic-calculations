using Symbolic.Model.Base;
using static Symbolic.Model.Template.Funcs;
using System;

namespace Symbolic.Model.Template
{
    public class Sinus : Function
    {
        private readonly Function _innerF;

        public Sinus() { }

        public Sinus(Function f)
        {
            _innerF = f;
        }

        public Function InnerF
        {
            get
            {
                return _innerF;
            }
        }

        /// <summary>
        /// Calculate function
        /// </summary>
        /// <param name="val"> Argument value </param>
        /// <returns> Function value </returns>
        public override double Calc(double val)
        {
            return Math.Sin(val);
        }

        /// <summary>
        /// Deirvative rule
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return  Cos(InnerF) * InnerF.Derivative();
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return  $"sin({InnerF})";
        }

        /// <summary>
        /// Latex view
        /// </summary>
        /// <returns></returns>
        public override string ToLatexString()
        {
            return $@"\sin ({InnerF.ToLatexString()})";
        }

        #endregion
    }
}
