using System;
using Symbolic.Model.Base;

namespace Symbolic.Model.Template
{
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

        public override double Calc(double val)
        {
            return val;
        }

        /// <summary>
        /// Deirvative RULE
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return Funcs.Zero + 1;
        }

        public override string ToString()
        {
            return $"x";
        }

        public override string ToLatexString()
        {
            return $"x";
        }
    }
}
