using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using static Symbolic.Model.Template.Funcs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template.Hyperbolic
{
    class HypTangens : Function
    {
        private readonly Function _innerF;

        public HypTangens() { }

        public HypTangens(Function f)
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
            return Math.Tanh(val);
        }

        /// <summary>
        /// Deirvative rule
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return 1 - Tgh(InnerF) ^ 2 * InnerF.Derivative();
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"(exp({InnerF.ToLatexString()}) - exp({((-1) * InnerF).ToLatexString()}))/(exp({InnerF.ToLatexString()}) + exp({((-1) * InnerF).ToLatexString()}))";
        }

        public override string ToLatexString()
        {
            return $@"(\exp ({InnerF.ToLatexString()}) - \exp ({((-1) * InnerF).ToLatexString()}))/(\exp ({InnerF.ToLatexString()}) - \exp ({((-1) * InnerF).ToLatexString()}))";
        }

        #endregion
    }
}
