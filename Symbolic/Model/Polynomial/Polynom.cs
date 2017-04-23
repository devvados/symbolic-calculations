using Symbolic.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Polynomial
{
    class Polynom : Function, ICloneable
    {
        /// <summary>
        /// Список мономов
        /// </summary>
        public List<Monom> monoms;

        #region Свойства

        public Monom LT
        {
            get
            {
                if (monoms.Count < 1)
                {
                    return null;
                }
                else
                {
                    return monoms.First();
                }
            }
        }

        public Monom LM
        {
            get
            {
                return new Monom(1, LT.Powers);
            }
        }

        public double LC
        {
            get
            {
                return LT.Coef;
            }
        }

        public bool IsNull
        {
            get
            {
                if (Degree == 0)
                    return true;
                else
                    return false;
            }
        }

        public int Degree
        {
            get
            {
                int max = 0;
                for (int i = 0; i < monoms.Count; i++)
                {
                    int sum = 0;
                    foreach (int j in monoms[i].Powers)
                        sum += j;

                    if (sum > max)
                        max = sum;
                }

                return max;
            }
        }

        public List<int> Multideg
        {
            get
            {
                return LT.Powers;
            }
        }

        public Monom this[int index]
        {
            get
            {
                return monoms[index];
            }
            set
            {
                monoms[index] = value;
            }
        }

        #endregion

        #region Конструкторы

        public Polynom()
        {
            monoms = new List<Monom>();
        }

        public Polynom(List<Monom> ms)
        {
            monoms = new List<Monom>(ms);
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
                monoms = new List<Monom>(this.monoms)
            };
        }

        /// <summary>
        /// Упрощение
        /// </summary>
        /// <returns> Упрощенный полином </returns>
        public Polynom SimplifyPolynom()
        {
            List<int> nullCoefs = new List<int>();

            for (int i = 0; i < monoms.Count; i++)
            {
                //найдем нулевые коэффициенты
                if (monoms[i].Coef == 0)
                {
                    monoms.RemoveAt(i);
                    //nullCoefs.Add(i);
                    i--;
                }
            }

            for (int i = 0; i < monoms.Count; i++)
            {
                for (int j = i + 1; j < monoms.Count; j++)
                {
                    if (Monom.AreEqual(monoms[i], monoms[j]))
                    {
                        monoms[i] = monoms[i] + monoms[j];
                        monoms.RemoveAt(j);
                    }
                }
            }

            return new Polynom(monoms);
        }

        /// <summary>
        /// Строковое представление полинома
        /// </summary>
        /// <returns> Полином-строка </returns>
        public override string ToString()
        {
            StringBuilder stringPoly = new StringBuilder();

            if (monoms.Count > 0)
            {
                for (int i = 0; i < monoms.Count; i++)
                {
                    stringPoly.Append(monoms[i].ToString());
                    if ((i + 1) < monoms.Count)
                    {
                        stringPoly.Append(" + ");
                    }
                    else if ((i + 1) == monoms.Count)
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

            if (variable > monoms.Count)
            {
                monoms.Clear();
            }
            else
            {
                foreach (Monom m in monoms)
                {
                    if (m.Powers[variable] == 0)
                    {
                        monoms.RemoveAt(variable);
                    }
                    else
                    {
                        m.Coef *= m.Powers[variable];
                        m.Powers[variable] -= 1;
                    }
                }

            }

            return new Polynom(monoms);
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
            Polynom h = f.Degree > g.Degree ? (Polynom)f.Clone() : (Polynom)g.Clone();
            Polynom s = f.Degree < g.Degree ? (Polynom)f.Clone() : (Polynom)g.Clone();

            while (!s.IsNull)
            {
                if (h.Degree >= s.Degree)
                {
                    Polynom rem = new Polynom();
                    List<Monom> q = new List<Monom>();
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
            Polynom h = new Polynom();
            List<Polynom> sortedByDescending = polynoms.OrderBy(p => p.Degree).ToList();

            for (int i = sortedByDescending.Count - 2; i >= 0; i--)
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
                    Monom divLT = r.LT / g.LT;
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
            res.monoms.AddRange(a.monoms);
            res.monoms.AddRange(b.monoms);
            res.SimplifyPolynom();
            return res;
        }
        public static Polynom operator -(Polynom p)
        {
            Polynom res = (Polynom)p.Clone();
            foreach (Monom t in res.monoms)
            {
                t.Coef *= -1;
            }
            return res;
        }
        public static Polynom operator -(Polynom a, Polynom b)
        {
            Polynom res = new Polynom();
            res.monoms.AddRange(a.monoms);
            res.monoms.AddRange((-b).monoms);
            res.SimplifyPolynom();
            return res;
        }
        public static Polynom operator *(Polynom p, Monom m)
        {
            Polynom tempPoly = (Polynom)p.Clone();
            for (int i = 0; i < tempPoly.monoms.Count; i++)
            {
                tempPoly.monoms[i] *= m;
            }
            return tempPoly;
        }

        #endregion
    }
}
