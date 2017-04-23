using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Polynomial
{
    class RationalFunction : Function
    {
        /// <summary>
        /// Числитель и знаменатель
        /// </summary>
        public Polynom numerator, denominator;

        public RationalFunction(Polynom num, Polynom denom)
        {
            numerator = num;
            denominator = denom;
        }

        #region Наследование от класса Function

        /// <summary>
        /// Для галочки
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public override double Calc(double val)
        {
            return 0;
        }

        /// <summary>
        /// Для галочки
        /// </summary>
        /// <returns></returns>
        public override Function Derivative()
        {
            return null;
        }

        #endregion

        /// <summary>
        /// Подсчет значения функции в точке
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public override double Calc(params double[] val)
        {
            return 0;
        }

        /// <summary>
        /// Производная по переменной (порядковый ее номер)
        /// </summary>
        /// <param name="varnum">Строго больше 0</param>
        /// <returns></returns>
        public override Function Derivative(int varnum)
        {
            return null;
        }

        /// <summary>
        /// Упростить рациональную функцию
        /// </summary>
        /// <param name="divisionResult"></param>
        /// <returns></returns>
        public Function Simplify(out List<Monom> divisionResult)
        {
            divisionResult = new List<Monom>();

            if (numerator.LT.CompareTo(denominator.LT) >= 0) //сравниваем степени числителя и знаменателя
            {
                var reminder = new Polynom();
                Polynom.DividePolynoms(numerator, denominator, out divisionResult, out reminder);
                return new RationalFunction(reminder, denominator);
            }
            else
                return new RationalFunction(numerator, denominator); 
        }

        /// <summary>
        /// Строковое представление рациональной функции
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{numerator.ToString()}/{denominator.ToString()}";
        }
    }
}
