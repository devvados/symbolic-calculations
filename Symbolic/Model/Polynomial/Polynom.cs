using MathNet.Symbolics;
using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Expr = MathNet.Symbolics.Expression;
using PolyLib;
using Symbolic.Model.Parser;

/// <summary>
/// Polynomial class
/// </summary>
namespace Symbolic.Model.Polynomial
{
    /// <summary>
    /// For deep copy
    /// </summary>
    static class Extensions
    {
        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }

    /// <summary>
    /// Pair polynom-power
    /// </summary>
    class Pair
    {
        int power;
        Polynom p;    

        public Pair(int pow, Polynom poly)
        {
            P = poly;
            Power = pow;
        }

        public int Power
        {
            get
            {
                return power;
            }

            set
            {
                power = value;
            }
        }

        public Polynom P
        {
            get
            {
                return p;
            }

            set
            {
                p = value;
            }
        }
    }

    class Polynom : Base.Function, ICloneable
    {
        /// <summary>
        /// Monoms list
        /// </summary>
        public List<Monom> Monoms;

        #region Properties

        public Monom LT => Monoms.Count < 1 ? null : Monoms.First();

        public Monom LM => new Monom(1, LT.Powers);

        public double LC => LT.Coef;

        public bool IsNull => Degree == 0;

        public int Degree
        {
            get
            {
                var max = 0;
                foreach (var t in Monoms)
                {
                    var sum = 0;
                    for (var i = 0; i < t.Powers.Count; i++)
                    {
                        sum += t.Powers[i].Item2;
                    }

                    if (sum > max)
                        max = sum;
                }
                return max;
            }
        }

        public List<Tuple<string, int>> Multideg => LT.Powers;

        public new Monom this[int index]
        {
            get { return Monoms[index]; }
            set { Monoms[index] = value; }
        }

        #endregion

        #region Constructors

        public Polynom()
        {
            Monoms = new List<Monom>();
        }

        public Polynom(List<Monom> ms)
        {
            Monoms = new List<Monom>(ms);
        }

        #endregion

        #region Derivative, Integral, Calculation

        /// <summary>
        /// Derivative
        /// </summary>
        /// <param name="varnum"> Variable </param>
        /// <returns> Polynom </returns>
        public override Base.Function Derivative()
        {
            var f = Infix.ParseOrThrow(this.ToString());
            var der = Calculus.Differentiate(Expr.Symbol("x"), f);
            var poly = PolynomParser.Parse(Infix.Format(der));

            return poly;
        }

        /// <summary>
        /// Calculate function
        /// </summary>
        /// <param name="val"> Argument value </param>
        /// <returns> Function value </returns>
        public override double Calc(params double[] val)
        {
            return 0;
        }

        #endregion

        /// <summary>
        /// Copy
        /// </summary>
        /// <returns> Object </returns>
        public object Clone()
        {
            return new Polynom
            {
                Monoms = new List<Monom>(Monoms)
            };
        }

        /// <summary>
        /// Simplifying
        /// </summary>
        /// <returns> Polynom </returns>
        public Polynom SimplifyPolynom()
        {
            var nullCoefs = new List<int>();

            for (var i = 0; i < Monoms.Count; i++)
            {
                //найдем нулевые коэффициенты
                if (Monoms[i].Coef == 0)
                {
                    Monoms.RemoveAt(i);
                    //nullCoefs.Add(i);
                    i--;
                }
            }

            for (var i = 0; i < Monoms.Count; i++)
            {
                Monoms[i].Powers.RemoveAll(x => x.Item2 == 0);
            }

            for (var i = 0; i < Monoms.Count; i++)
            {
                for (var j = i + 1; j < Monoms.Count; j++)
                {
                    if (Monom.AreEqual(Monoms[i], Monoms[j]))
                    {
                        Monoms[i] = Monoms[i] + Monoms[j];
                        Monoms.RemoveAt(j);
                    }
                }
            }

            return new Polynom(Monoms);
        }

        #region Наибольший общий делитель

        /// <summary>
        /// GCD (f1, f2)
        /// </summary>
        /// <param name="f"> Polynom </param>
        /// <param name="g"> Polynom </param>
        /// <returns> Polynom </returns>
        public static Polynom GetGCD(Polynom f, Polynom g)
        {
            var h = f.Degree > g.Degree ? (Polynom)f.Clone() : (Polynom)g.Clone();
            var s = f.Degree < g.Degree ? (Polynom)f.Clone() : (Polynom)g.Clone();

            h = LexOrder.CreateOrderedPolynom(h);
            s = LexOrder.CreateOrderedPolynom(s);

            while (!s.IsNull)
            {
                if (h.Degree >= s.Degree)
                {
                    Polynom rem;
                    List<Monom> q;
                    DividePolynoms(h, s, out q, out rem);
                    h = s;
                    s = rem;
                }
                else
                    break;
            }

            return h;
        }

        /// <summary>
        /// GCD (f1, f2, ..., fn)
        /// </summary>
        /// <param name="polynoms"> Polynoms </param>
        /// <returns> Polynom </returns>
        public static Polynom GetGCD(params Polynom[] polynoms)
        {
            var h = new Polynom();
            var sortedByDescending = polynoms.OrderBy(p => p.Degree).ToList();

            for (var i = sortedByDescending.Count - 2; i >= 0; i--)
            {
                sortedByDescending[i] = GetGCD(sortedByDescending[i], sortedByDescending[i + 1]);
                sortedByDescending.RemoveAt(i + 1);
            }

            return sortedByDescending.First();
        }

        /// <summary>
        /// polynom division with reminder
        /// </summary>
        /// <param name="f"> Polynom </param>
        /// <param name="g"> Polunom </param>
        /// <param name="q"> Division </param>
        /// <param name="r"> Reminder </param>
        public static void DividePolynoms(Polynom f, Polynom g, out List<Monom> q, out Polynom r)
        {
            q = new List<Monom>();
            r = (Polynom)f.Clone();

            while (r.Degree >= g.Degree)
            {
                if (!r.IsNull && Monom.CanDivide(r.LT, g.LT))
                {
                    var divLT = r.LT / g.LT;
                    q.Add(divLT);
                    var temp = g * (divLT);
                    r = r - temp;
                    r = r.SimplifyPolynom();
                    r = LexOrder.CreateOrderedPolynom(r);                  
                }
                else
                    break;
            }
        }

        #endregion

        #region Print formula

        /// <summary>
        /// String view
        /// </summary>
        /// <returns> Полином-строка </returns>
        public override string ToString()
        {
            var stringPoly = new StringBuilder();

            if (Monoms.Count > 0)
            {
                for (var i = 0; i < Monoms.Count; i++)
                {
                    stringPoly.Append(Monoms[i]);
                    if ((i + 1) < Monoms.Count)
                    {
                        stringPoly.Append(" + ");
                    }
                    else if ((i + 1) == Monoms.Count)
                    {
                        break;
                    }
                }
            }
            else
            {
                stringPoly.Append("0");
            }
            return stringPoly.ToString();
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

        /// <summary>
        /// S-polynom
        /// </summary>
        /// <param name="a"> Polynom </param>
        /// <param name="b"> Polynom </param>
        /// <returns></returns>
        public static Polynom S_polynom(Polynom a, Polynom b)
        {
            var lcm = Monom.GetLCM(a.LT, b.LT);
            var sp = a * (lcm / a.LT) - b * (lcm / b.LT);
            return sp;
        }

        /// <summary>
        /// Polynom Resultant
        /// </summary>
        /// <param name="a"> Polynom </param>
        /// <param name="b"> Polynom </param>
        /// <returns></returns>
        public static Polynom Resultant(Polynom a, Polynom b)
        {
            List<Pair> polyList = new List<Pair>();
            Polynom tempA = LexOrder.CreateOrderedPolynom(new Polynom(Extensions.Clone(a.Monoms))), 
                    tempB = LexOrder.CreateOrderedPolynom(new Polynom(Extensions.Clone(b.Monoms)));
            var matrixArray = new Polynom[][] { };

            var b1 = LexOrder.CreateOrderedPolynom(b);
            var a1 = LexOrder.CreateOrderedPolynom(a);
            a1.Monoms.RemoveAll(x => x.Coef == 0);
            b1.Monoms.RemoveAll(x => x.Coef == 0);

            int max1 = FindMaxPower(a), max2 = FindMaxPower(b);

            //Сгруппированные выражения по степеням х
            List<Pair> polyList1 = GroupCoefficients(tempA),
                       polyList2 = GroupCoefficients(tempB);
            var filledPairs1 = FillPairList(polyList1);
            var filledPairs2 = FillPairList(polyList2);

            List<List<Polynom>> matrix = new List<List<Polynom>>();
            //Построение части матрицы
            int step = 0;
            for (var i = 0; i < max2; i++)
            {
                step = i;
                var row = new List<Polynom>(new Polynom[a1.LT.GetPower+b1.LT.GetPower]);

                row.RemoveRange(0, filledPairs1.Count);
                row.InsertRange(step, filledPairs1.Select(x => x.P));

                FillPolynom(row);
                matrix.Add(row);
                step++;
            }
            //Построение части матрицы
            step = 0;
            for (var i = 0; i < max1; i++)
            {
                step = i;
                var row = new List<Polynom>(new Polynom[a1.LT.GetPower + b1.LT.GetPower]);
                row.RemoveRange(0, filledPairs2.Count);
                row.InsertRange(step, filledPairs2.Select(x => x.P));

                FillPolynom(row);
                matrix.Add(row);
                step++;
            }
            
            Matrix m = new Matrix(matrix);
            var det = m.Determinant();
            
            return det;   
        }

        /// <summary>
        /// Roots of a polynom
        /// </summary>
        /// <param name="a"> Polynom </param>
        /// <returns></returns>
        public static List<double> FindRoots(Polynom a)
        {
            List<double> roots = new List<double>();

            WolframAlphaNET.WolframAlpha w = new WolframAlphaNET.WolframAlpha("RAG9YE-E5PVQUEEKT");
            var res = w.Query(a.ToString() + " = 0");

            if (res != null)
            {
                var s = res.Pods[1].SubPods.ToList();
                foreach (var s1 in s)
                {
                    if (s1.Plaintext.Contains("x"))
                    {
                        var s2 = s1.Plaintext.Replace("x = ", "");
                        if (s2.Contains("/"))
                        {
                            roots.Add(FromFraction(s2));
                        }
                        else
                        {
                            roots.Add(Convert.ToDouble(s2));
                            var v = new PolyLib.Polynomial(-1, 1).Roots();
                        }
                    }
                }
            }
  
            return roots;
        }

        /// <summary>
        /// Fraction to decimal
        /// </summary>
        /// <param name="str"> Fraction string </param>
        /// <returns></returns>
        public static double FromFraction(string str)
        {
            string[] str1 = str.Split('/');
            double d = Convert.ToDouble(str1[0]) / Convert.ToDouble(str1[1]);
            return d;
        }

        /// <summary>
        /// Fill polynom with missing powers
        /// </summary>
        /// <param name="row"> Polynoms </param>
        public static void FillPolynom(List<Polynom> row)
        {
            for (var k = 0; k < row.Count; k++)
            {
                if (row[k] == null)
                    row[k] = new Polynom(new List<Monom> { new Monom(0) });
            }
        }

        /// <summary>
        /// Group polynom coefficients
        /// </summary>
        /// <param name="a"> Polynom </param>
        /// <returns></returns>
        public static List<Pair> GroupCoefficients(Polynom a)
        {
            List<Pair> polyList = new List<Pair>();
            Dictionary<int, Polynom> polyDict = new Dictionary<int, Polynom>();

            //разделение на слагаемые
            var subList = a.Monoms.FindAll(x => x.Powers.FindAll(y => y.Item1.Contains("x")) != null);
            
            //группировка по степеням
            foreach (var item in subList)
            {
                var pow = item.GetPower;
                if (!polyDict.Keys.ToList().Contains(pow))
                {
                    var p = new Polynom();
                    item.Powers.RemoveAll(x => x.Item1.Contains("x"));
                    p.Monoms.Add(item);
                    polyDict.Add(pow, p);
                }
                else
                {
                    item.Powers.RemoveAll(x => x.Item1.Contains("x"));
                    polyDict[pow].Monoms.Add(item);
                }
            }
            polyDict.OrderByDescending(x => x.Key);

            foreach (var kvp in polyDict)
            {
                polyList.Add(new Pair(kvp.Key, kvp.Value));
            }

            return polyList;
        }
        
        /// <summary>
        /// Get polynom max power
        /// </summary>
        /// <param name="a"> Polynom param>
        /// <returns></returns>
        public static int FindMaxPower(Polynom a)
        {
            int max = 0;
            foreach (var m in a.Monoms)
            {
                if (m.Powers.Count > 0)
                {
                    int tempMax = m.GetPower;
                    if (tempMax > max)
                        max = tempMax;
                }
            }
            return max;
        }

        public static List<Pair> FillPairList(List<Pair> pair)
        {
            int maxPow = pair.Max(x => x.Power);
            for (var i = 0; i < maxPow; i++)
            {
                if (pair.Find(x => x.Power == i) == null)
                {
                    pair.Add(new Pair(i, new Polynom()));
                }
            }
            return pair.OrderByDescending(x => x.Power).ToList();
        }

        #region Unary/binary operators

        public static Polynom operator +(Polynom a, Polynom b)
        {
            Polynom res = new Polynom();
            res.Monoms.AddRange(a.Monoms);
            res.Monoms.AddRange(b.Monoms);
            res.SimplifyPolynom();
            return res;
        }

        public static Polynom operator -(Polynom p)
        {
            Polynom res = (Polynom)p.Clone();
            foreach (Monom t in res.Monoms)
            {
                t.Coef *= -1;
            }
            return res;
        }

        public static Polynom operator -(Polynom a, Polynom b)
        {
            Polynom res = new Polynom();
            res.Monoms.AddRange(a.Monoms);
            res.Monoms.AddRange((-b).Monoms);
            res.SimplifyPolynom();
            return res;
        }

        public static Polynom operator *(Polynom p, Monom m)
        {
            Polynom tempPoly = (Polynom)p.Clone();
            for (int i = 0; i < tempPoly.Monoms.Count; i++)
            {
                tempPoly.Monoms[i] *= m;
            }
            return tempPoly;
        }

        #endregion
    }
}
