using Symbolic.Model.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Base
{
    public abstract class Function
    {
        /// <summary>
        /// Значение функции в точке
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public abstract double Calc(double val);

        /// <summary>
        /// Для полинома
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual double Calc(params double[] val)
        {
            return 0;
        }

        /// <summary>
        /// Производная функции
        /// </summary>
        /// <returns></returns>
        public abstract Function Derivative();

        /// <summary>
        /// Для полинома
        /// </summary>
        /// <param name="varnum"></param>
        /// <returns></returns>
        public virtual Function Derivative(int varnum)
        {
            return null;
        }

        /// <summary>
        /// n - derivative of function
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public Function this[int n]
        {
            get
            {
                if (n < 0)
                    throw new ArgumentException("n must be more than zero");
                var f = this;
                for (int i = 0; i < n; ++i)
                    f = f.Derivative();
                return f;
            }
        }

        #region f(x) <OPERATOR> g(x)

        /// <summary>
        /// f(x) ^ g(x)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Function operator ^(Function a, Function b)
        {
            return Power.New(a, b);
        }

        /// <summary>
        /// f(x) + g(x)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Function operator +(Function a, Function b)
        {
            return Addition.New(a, b);
        }

        /// <summary>
        /// f(x) - g(x)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Function operator -(Function a, Function b)
        {
            return Difference.New(a, b);
        }

        /// <summary>
        /// f(x) * g(x)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Function operator *(Function a, Function b)
        {
            return Multiplication.New(a, b);
        }

        /// <summary>
        /// f(x) / g(x)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Function operator /(Function a, Function b)
        {
            return Division.New(a, b);
        }

        /// <summary>
        /// Composition operator
        /// </summary>
        /// <returns>Composition.New(a, b)</returns>
        public static Function operator %(Function a, Function b)
        {
            return Composition.New(a, b);
        }

        #endregion

        #region f(x) <OPERATOR> K

        /// <summary>
        /// f(x) ^ n
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Function operator ^(Function a, int b)
        {
            return Power.New(a, new Constant(b));
        }

        /// <summary>
        /// k + f(x)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Function operator +(double k, Function b)
        {
            return new Constant(k) + b;
        }

        /// <summary>
        /// f(x) + k
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Function operator +(Function a, double k)
        {
            return a + new Constant(k);
        }

        /// <summary>
        /// k - f(x)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Function operator -(double k, Function b)
        {
            return new Constant(k) - b;
        }

        /// <summary>
        /// f(x) - k
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Function operator -(Function a, double k)
        {
            return a - new Constant(k);
        }

        /// <summary>
        /// k * f(x)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Function operator *(double k, Function b)
        {
            return new Constant(k) * b;
        }

        /// <summary>
        /// f(x) * k
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Function operator *(Function a, double k)
        {
            return a * new Constant(k);
        }

        /// <summary>
        /// k / f(x)
        /// </summary>
        /// <param name="k"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Function operator /(double k, Function b)
        {
            return new Constant(k) / b;
        }

        /// <summary>
        /// f(x) / k
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Function operator /(Function a, double k)
        {
            return a / new Constant(k);
        }

        #endregion
    }
}
