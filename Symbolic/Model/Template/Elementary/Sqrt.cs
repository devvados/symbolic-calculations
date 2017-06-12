using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template
{
    class Sqrt : Function
    {
        private readonly Function _innerF;

        public Sqrt() { }

        public Sqrt(Function f)
        {
            _innerF = f;
        }

        /// <summary>
        /// Производная функции
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return (1 / (2 * new Sqrt(_innerF))) * _innerF.Derivative();
        }

        #region Print formula

        /// <summary>
        /// Вывод в виде строки
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"sqrt({_innerF})";
        }

        public override string ToLatexString()
        {
            return $@"\sqrt ({_innerF.ToLatexString()})";;
        }

        #endregion  
    }
}
