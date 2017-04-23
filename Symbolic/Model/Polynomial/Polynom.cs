using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Symbolic.Model.Polynomial
{
    class Polynom : Function, ICloneable
    {
        /// <summary>
        /// Список мономов
        /// </summary>
        public List<Monom> Monoms;

        #region Свойства

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
                    var sum = t.Powers.Sum();

                    if (sum > max)
                        max = sum;
                }
                return max;
            }
        }

        public List<int> Multideg => LT.Powers;

        public Monom this[int index]
        {
            get => Monoms[index];
            set => Monoms[index] = value;
        }

        #endregion

        #region Конструкторы

        public Polynom()
        {
            Monoms = new List<Monom>();
        }

        public Polynom(List<Monom> ms)
        {
            Monoms = new List<Monom>(ms);
        }

        #endregion

        #region Наследование от класса Function

        public override Function Derivative()
        {
            throw new NotImplementedException();
        }

        public override double Calc(double val)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Копия объекта "Полином"
        /// </summary>
        /// <returns> Полная копия объекта </returns>
        public object Clone()
        {
            return new Polynom
            {
                Monoms = new List<Monom>(Monoms)
            };
        }

        /// <summary>
        /// Упрощение
        /// </summary>
        /// <returns> Упрощенный полином </returns>
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

        /// <summary>
        /// Строковое представление полинома
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
        /// Производная по какой-то переменной
        /// </summary>
        /// <param name="varnum"> Порядок переменной </param>
        /// <returns></returns>
        public override Function Derivative(int varnum)
        {
            int variable = varnum - 1;

            if (variable > Monoms.Count)
            {
                Monoms.Clear();
            }
            else
            {
                foreach (Monom m in Monoms)
                {
                    if (m.Powers[variable] == 0)
                    {
                        Monoms.RemoveAt(variable);
                    }
                    else
                    {
                        m.Coef *= m.Powers[variable];
                        m.Powers[variable] -= 1;
                    }
                }

            }

            return new Polynom(Monoms);
        }

        /// <summary>
        /// Вычисление значения в точке (x1, x2, ..., xn)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public override double Calc(params double[] val)
        {
            return 0;
        }

        /// <summary>
        /// Наибольший общий делитель 2-х полиномов
        /// </summary>
        /// <param name="f"> Полином </param>
        /// <param name="g"> Полином </param>
        /// <returns> НОД - полином </returns>
        public static Polynom GetGCD(Polynom f, Polynom g)
        {
            var h = f.Degree > g.Degree ? (Polynom)f.Clone() : (Polynom)g.Clone();
            var s = f.Degree < g.Degree ? (Polynom)f.Clone() : (Polynom)g.Clone();

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
        /// Наибольший общий делитель 3 и более полиномов
        /// </summary>
        /// <param name="polynoms"> Список полиномов </param>
        /// <returns></returns>
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
        /// Деление полиномов с остатком
        /// </summary>
        /// <param name="f"> Делимое </param>
        /// <param name="g"> Делитель </param>
        /// <param name="q"> Частное </param>
        /// <param name="r"> Остаток </param>
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
                    r = LexOrder.CreateOrderedPolynom(r);
                    r = r.SimplifyPolynom();
                }
                else
                    break;
            }
        }

        /// <summary>
        /// S-полином
        /// </summary>
        /// <param name="a"> Полином </param>
        /// <param name="b"> Полином </param>
        /// <returns></returns>
        public static Polynom S_polynom(Polynom a, Polynom b)
        {
            var lcm = Monom.GetLCM(a.LT, b.LT);
            var sp = a * (lcm / a.LT) - b * (lcm / b.LT);
            return sp;
        }

        #region Операторы унарные/бинарные

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
