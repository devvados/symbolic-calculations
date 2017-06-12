using Symbolic.Model.Base;
using Symbolic.Model.Template.Trigonometric;
using Symbolic.Model.Template.InverseTrig;
using System;
using Symbolic.Model.Template.Hyperbolic;

namespace Symbolic.Model.Template
{
    public static class Funcs
    {
        public static Function func = new Identity();

        public static readonly Function Zero = new Constant(0);

        public static readonly Function Id = new Identity();

        #region Элементарные функции

        /// <summary>
        /// f(x) = x ^ n
        /// </summary>
        /// <param name="n">n >= 0</param>
        /// <returns></returns>
        public static Function PowerFunction(int n)
        {
            return PowerFunction(Id, n);
        }

        /// <summary>
        /// f(x) = g(x) ^ n
        /// </summary>
        /// <param name="g"></param>
        /// <param name="n">n >= 0</param>
        /// <returns></returns>
        public static Function PowerFunction(Function g, int n)
        {
            if (n == 0)
                return Zero + 1;
            var f = g;
            for (int i = 1; i < n; ++i)
                f *= g;
            return f;
        }

        /// <summary>
        /// e^(x) or e^(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Exp(Function f = null)
        {
            return new Exponenta(f);
        }

        /// <summary>
        /// Ln(x) or Ln(g(x)) if a = exp;
        /// Log[a](x) or Log[a](g(x)) if a != exp
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Function Ln(Function f = null, double a = Math.E)
        {
            return new Logarithm(f, a);
        }

        public static Function Sq(Function f = null)
        {
            return new Sqrt(f);
        }

        #endregion

        #region Тригонометрические функции

        /// <summary>
        /// Sin(x) or Sin(g(x)) 
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Sin(Function f = null)
        {
            return new Sinus(f);
        }

        /// <summary>
        /// Cos(x) or Cos(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Cos(Function f = null)
        {
            return new Cosinus(f);
        }

        /// <summary>
        /// Tan(x) or Tan(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Tan(Function f = null)
        {
            return new Tangens(f);
        }

        /// <summary>
        /// Cth(x) or Ctg(g(x))
        /// </summary>
        public static Function Cot(Function f = null)
        {
            return new Cotangens(f);
        }

        #endregion

        #region Обратные тригонометрические функции

        /// <summary>
        /// Arcsin(x) or Arcsin(g(x)) 
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Asin(Function f = null)
        {
            return new Arcsinus(f);
        }

        /// <summary>
        /// Arccos(x) or Arccos(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Acos(Function f = null)
        {
            return new Arccosinus(f);
        }

        /// <summary>
        /// Arctan(x) or Arctan(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Atan(Function f = null)
        {
            return new Arctangens(f);
        }

        /// <summary>
        /// Arccot(x) or Arccot(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Acot(Function f = null)
        {
            return new Arccotangens(f);
        }

        #endregion

        #region Гиперболические функции

        /// <summary>
        /// Sh(x) or Sh(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Sh(Function f = null)
        {
            return new HypSinus(f);
        }

        /// <summary>
        /// Ch(x) or Ch(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Ch(Function f = null)
        {
            return new HypCosinus(f);
        }

        /// <summary>
        /// Tgh(x) or Tgh(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Tgh(Function f = null)
        {
            return new HypTangens(f);
        }

        /// <summary>
        /// Cth(x) or Cth(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Cth(Function f = null)
        {
            return new HypCotangens(f);
        }

        #endregion

        #region Численное интегрирование

        /// <summary>
        /// Definite Integral
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Integrate(this Function f, double a, double b)
        {
            return Integrate(f, a, b, (b - a) * 0.00001);
        }

        public static double Integrate(this Function f, double a, double b, double delta)
        {
            if (Math.Sign(delta) != Math.Sign(b - a))
                delta *= -1;

            double sum = 0;
            
            for (var x = a; x < b; x += delta)
            {
                if (x + delta > b)
                {
                    sum += (b - x) * f.Calc(x);
                    break;
                }
                sum += delta * f.Calc(x);
            }
            return sum;
        }

        /// <summary>
        /// Simpson Method
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double SimpsonIntegrate(this Function f, double a, double b)
        {
            return SimpsonIntegrate(f, a, b, (b - a) * 0.00001);
        }

        public static double SimpsonIntegrate(this Function f, double a, double b, double delta)
        {
            if (Math.Sign(delta) != Math.Sign(b - a))
                delta *= -1;
            var n = (int)((b - a) / delta);
            var h = (b - a) / n;
            double sum = 0;
            sum = f.Calc(a);

            for (var i = 1; i <= n; i += 2)
            {
                var x = a + h * i;
                sum += 4 * f.Calc(x);
            }
            for (var i = 2; i <= n - 1; i += 2)
            {
                var x = a + h * i;
                sum += 2 * f.Calc(x);
            }
            sum += f.Calc(b);
     
            return h * sum / 3;
        }

        #endregion  
    }
}
