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
            if (_innerF != null)
                return (1 / (2 * new Sqrt(_innerF))) * _innerF.Derivative();
            else
                return 1 / (2 * new Sqrt());
        }

        #region Print formula

        /// <summary>
        /// Вывод в виде строки
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_innerF != null)
            {
                return $"sqrt({_innerF})";
            }
            else
                return $"sqrt(x)";
        }

        #endregion  
    }
}
