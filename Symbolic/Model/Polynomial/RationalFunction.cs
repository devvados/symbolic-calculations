using Symbolic.Model.Base;
using Symbolic.Model.Parser;
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
        public Polynom Numerator, Denominator;

        public RationalFunction(Polynom num, Polynom denom)
        {
            Numerator = num;
            Denominator = denom;
        }

        public RationalFunction()
        {
        }

        #region Derivative, Calculation

        /// <summary>
        /// Calculate function
        /// </summary>
        /// <param name="val"> Argument value </param>
        /// <returns> Function value </returns>
        public override double Calc(double val)
        {
            return 0;
        }

        /// <summary>
        /// Derivative
        /// </summary>
        /// <param name="varnum"> Variable </param>
        /// <returns> Rational Function </returns>
        public override Function Derivative()
        {
            return null;
        }

        #endregion

        /// <summary>
        /// Simplifying (division)
        /// </summary>
        /// <param name="divisionResult"> Polynom </param>
        /// <returns></returns>
        public Function Simplify(out List<Monom> divisionResult)
        {
            divisionResult = new List<Monom>();

            //Compare numerator and denominator degrees
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
        /// Rothstein and Trager Method
        /// </summary>
        /// <returns> Function </returns>
        public Function Integrate()
        {
            var denomimatorDerivative = PolynomParser.Parse(Denominator.Derivative().ToString());
            var arg2 = Numerator - (denomimatorDerivative * 
                new Monom(1, new List<Tuple<string, int>>()
                {
                    new Tuple<string, int>("c", 1)
                }));

            //Resultant
            var res = Polynom.Resultant(Denominator, arg2);

            //Roots
            var roots = Polynom.FindRoots(res);
            
            List <Tuple<Polynom, double>> GCDs = new List<Tuple<Polynom, double>>();
            foreach (var root in roots)
            {
                GCDs.Add(new Tuple<Polynom, double>(Polynom.GetGCD(Denominator, Numerator - (denomimatorDerivative * new Monom(root))), root));
            }

            //Get terms
            List<Function> funcs = new List<Function>();
            foreach (var pair in GCDs)
            {
                funcs.Add(new Constant(pair.Item2) * Funcs.Ln(pair.Item1));
            }

            //Build function
            Function result = new Constant(0);
            foreach (var f in funcs)
            {
                result += f;
            }

            return result;
        }

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({Numerator})/({Denominator})";
        }

        /// <summary>
        /// Latex view
        /// </summary>
        /// <returns></returns>
        public override string ToLatexString()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
