using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template.InverseTrig
{
    class Arctangens : Function
    {
        private readonly Function _innerF;

        public Arctangens() { }

        public Arctangens(Function f)
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
            return MathNet.Numerics.Trig.Atan(val);
        }

        /// <summary>
        /// Deirvative rule
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return (1 / (1 + (InnerF ^ 2))) * InnerF.Derivative();
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"arctan({InnerF})";
        }

        public override string ToLatexString()
        {
            return $@"\arctan ({InnerF.ToLatexString()})";
        }

        #endregion
    }
}
