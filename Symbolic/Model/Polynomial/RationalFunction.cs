using Symbolic.Model.Base;
using Symbolic.Model.Template;
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
        public Polynom Numerator, Denominator;

        public RationalFunction(Polynom num, Polynom denom)
        {
            Numerator = num;
            Denominator = denom;
        }

        public RationalFunction()
        {
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

            //сравниваем степени числителя и знаменателя
            if (Numerator.LT.CompareTo(Denominator.LT) >= 0) 
            {
                Polynom reminder;
                Polynom.DividePolynoms(Numerator, Denominator, out divisionResult, out reminder);
                return new RationalFunction(reminder, Denominator);
            }
            else
                return new RationalFunction(Numerator, Denominator); 
        }

        /// <summary>
        /// Строковое представление рациональной функции
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({Numerator})/({Denominator})";
        }

        /// <summary>
        /// Интегрирование методом Ротштейна и Трагера
        /// </summary>
        /// <returns></returns>
        public Function Integrate()
        {
            var denomimatorDerivative = PolynomParser.Parse(Denominator.Derivative().ToString());
            var arg2 = Numerator - (denomimatorDerivative * 
                new Monom(1, new List<Tuple<string, int>>()
                {
                    new Tuple<string, int>("c", 1)
                }));

            //Поиск результанта
            var res = Polynom.Resultant(Denominator, arg2);

            //Поиск корней
            var roots = Polynom.FindRoots(res);
            
            List <Tuple<Polynom, double>> GCDs = new List<Tuple<Polynom, double>>();
            foreach (var root in roots)
            {
                GCDs.Add(new Tuple<Polynom, double>(Polynom.GetGCD(Denominator, Numerator - (denomimatorDerivative * new Monom(root))), root));
            }

            List<Function> funcs = new List<Function>();
            foreach (var pair in GCDs)
            {
                funcs.Add(new Constant(pair.Item2) * Funcs.Ln(pair.Item1));
            }

            Function result = new Constant(0);
            foreach (var f in funcs)
            {
                result += f;
            }

            return result;
        }
    }
}
