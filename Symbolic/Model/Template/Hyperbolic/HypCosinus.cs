using Symbolic.Model.Base;
using static Symbolic.Model.Template.Funcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template.Hyperbolic
{
    class HypCosinus : Function
    {
        private readonly Function _innerF;

        public HypCosinus() { }

        public HypCosinus(Function f)
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

        public override double Calc(double val)
        {
            return Math.Cosh(val);
        }

        /// <summary>
        /// Deirvative rule
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return Sh(InnerF) * InnerF.Derivative();
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"0.5 * (exp({InnerF}) - exp({(-1)*InnerF}))";
        }

        public override string ToLatexString()
        {
            return $@"0.5 \cdot (\exp ({InnerF.ToLatexString()}) - \exp ({((-1) * InnerF).ToLatexString()}))";
        }

        #endregion
    }
}
