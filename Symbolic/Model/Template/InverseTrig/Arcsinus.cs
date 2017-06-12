using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Symbolic.Model.Template.Funcs;
using System.Threading.Tasks;

namespace Symbolic.Model.Template.InverseTrig
{
    class Arcsinus : Function
    {
        private readonly Function _innerF;

        public Arcsinus() { }

        public Arcsinus(Function f)
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
            return MathNet.Numerics.Trig.Asin(val);
        }

        /// <summary>
        /// Deirvative rule
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return (1 / Sq(1 - (InnerF ^ 2))) * InnerF.Derivative();
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"arcsin({InnerF})";
        }

        public override string ToLatexString()
        {
            return $@"\arcsin ({InnerF.ToLatexString()})";
        }

        #endregion
    }
}
