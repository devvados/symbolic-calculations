using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Symbolic.Model.Template.Funcs;
using System.Threading.Tasks;

namespace Symbolic.Model.Template.Trigonometric
{   
    public class Tangens : Function
    {
        private readonly Function _innerF;

        public Tangens() { }

        public Tangens(Function f)
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
            return MathNet.Numerics.Trig.Tan(val);
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
            return $"tan({InnerF})";
        }

        public override string ToLatexString()
        {
            return $@"\tan ({InnerF.ToLatexString()})";
        }

        #endregion
    }
}
