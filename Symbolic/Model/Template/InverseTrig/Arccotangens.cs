using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template.InverseTrig
{
    class Arccotangens : Function
    {
        private readonly Function _innerF;

        public Arccotangens() { }

        public Arccotangens(Function f)
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
            return MathNet.Numerics.Trig.Acot(val);
        }

        /// <summary>
        /// Deirvative rule
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return (-1) * (1 / (1 + (InnerF ^ 2))) * InnerF.Derivative();
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"arccot({InnerF})";
        }

        /// <summary>
        /// Latex view
        /// </summary>
        /// <returns></returns>
        public override string ToLatexString()
        {
            return $@"\arccot ({InnerF.ToLatexString()})";
        }

        #endregion
    }
}
