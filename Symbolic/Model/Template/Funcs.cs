using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Template
{
    public static class Funcs
    {
        public static readonly Function Zero = new Constant(0);

        public static readonly Function Id = new Identity();

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

        #region Trigonometric functions

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
            return Sin(f) / Cos(f);
        }

        /// <summary>
        /// Cth(x) or Ctg(g(x))
        /// </summary>
        public static Function Ctg(Function f = null)
        {
            return Cos(f) / Sin(f);
        }

        #endregion

        #region Hyperbolic functions

        /// <summary>
        /// Sh(x) or Sh(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Sh(Function f = null)
        {
            return (Exp(f) - 1 / Exp(f)) / 2;
        }

        /// <summary>
        /// Ch(x) or Ch(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Ch(Function f = null)
        {
            return (Exp(f) + 1 / Exp(f)) / 2;
        }

        /// <summary>
        /// Tgh(x) or Tgh(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Tgh(Function f = null)
        {
            return Sh(f) / Ch(f);
        }

        /// <summary>
        /// Cth(x) or Cth(g(x))
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Function Cth(Function f = null)
        {
            return Sh(f) / Ch(f);
        }

        #endregion

        #region Distributions

        /// <summary>
        /// Fa(x) = 0 if x less than 0, and 1 - e^(-a*x) otherwise;
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Function NormalDistribition(double a = 1)
        {
            return Iplus * (1 - (Exp() % (-a * Id)));
        }

        public static Function UniformDistribution(double a, double b)
        {
            return new IntervalFunction(a, b) * ((Id - a) / (b - a)) + new IntervalFunction(b, double.PositiveInfinity);
        }

        #endregion

        #region Numeric Integration

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
            int i = 0;
            for (double x = a; x < b; x += delta)
            {
                if (x + delta > b)
                {
                    sum += (b - x) * f.Calc(x);
                    break;
                }
                sum += delta * f.Calc(x);
                ++i;
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
            int n = (int)((b - a) / delta);
            double h = (b - a) / n;
            double sum = 0;
            sum = f.Calc(a);
            for (int i = 1; i <= n; i += 2)
            {
                double x = a + h * i;
                sum += 4 * f.Calc(x);
            }
            for (int i = 2; i <= n - 1; i += 2)
            {
                double x = a + h * i;
                sum += 2 * f.Calc(x);
            }
            sum += f.Calc(b);
            /*int i = 0;
            for (double x = a; x < b; x += delta)
            {
                if (x + delta > b)
                {
                    delta = b - a;
                }
                sum += delta * (f.Calc(x) + 4 * f.Calc((2 * x + delta) / 2) + f.Calc(x + delta));
                ++i;
            }
            sum /= 6;*/
            return h * sum / 3;
        }

        #endregion

        #region Interval functions

        /// <summary>
        /// Interval from 0 to +Infinity
        /// </summary>
        public static readonly Function Iplus = new IntervalFunction(0, double.PositiveInfinity);

        /// <summary>
        /// Interval from -Infinity to 0
        /// </summary>
        public static readonly Function Iminus = new IntervalFunction(double.NegativeInfinity, 0);

        #endregion        

        /// <summary>
        /// Signum
        /// </summary>
        public static readonly Function Sign = Iplus - Iminus;

        public static readonly Function Abs = Sign * Id;
    }
}
