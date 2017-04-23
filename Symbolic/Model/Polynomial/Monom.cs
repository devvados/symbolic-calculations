using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Polynomial
{
    /// <summary>
    /// Класс Моном
    /// </summary>
    class Monom : IComparable<Monom>, ICloneable
    {
        /// <summary>
        /// коэффициент и список степеней
        /// </summary>
        private double _coef;
        public List<int> Powers;

        #region Свойства

        public double Coef
        {
            get
            {
                return _coef;
            }
            set
            {
                _coef = value;
            }
        }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Пустой
        /// </summary>
        public Monom()
        {
            _coef = 1;
            Powers = new List<int>();
        }

        /// <summary>
        /// Свободный член
        /// </summary>
        /// <param name="k"> Коэффициент </param>
        public Monom(int k)
        {
            _coef = k;
            Powers = new List<int>();
        }

        /// <summary>
        /// Моном + коэффициент
        /// </summary>
        /// <param name="k"> Коэффициент </param>
        /// <param name="pow"> Список степеней </param>
        public Monom(double k, List<int> pow)
        {
            _coef = k;
            Powers = pow;
        }

        #endregion

        ///<summary> 
        ///Копия объекта "Моном" 
        ///</summary>
        public object Clone()
        {
            return new Monom
            {
                Coef = this.Coef,
                Powers = new List<int>(Powers)
            };
        }

        ///<summary>
        ///Сравнение мономов 
        ///</summary>
        ///<param name="b"> Аргумент - моном, с которым сравниваем </param>
        ///<returns> 1- сравниваемый больше, 0 - равны, -1 - сравниваемый меньше </returns>
        public virtual int CompareTo(Monom b)
        {
            int compared = 0;
            Monom tempMonom = new Monom();

            //если сравниваем с тем же
            if (Powers == b.Powers)
                return 0;

            //Количество переменных в мономе
            if (Powers.Count > b.Powers.Count)
                b.CompleteMonom(this);
            else if (Powers.Count < b.Powers.Count)
                CompleteMonom(b);
            if (Powers.Count == b.Powers.Count)
            {
                tempMonom = this;
                for (int i = 0; i < tempMonom.Powers.Count; i++)
                {
                    if (Powers[i] > b.Powers[i])
                    {
                        //первый больше
                        compared = 1;
                        break;
                    }
                    else if (Powers[i] < b.Powers[i])
                    {
                        //второй больше
                        compared = -1;
                        break;
                    }
                    else if (Powers[i] == b.Powers[i])
                        continue;
                }
            }
            return compared;
        }

        /// <summary>
        /// Get monoms LCM
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Monom GetLCM(Monom a, Monom b)
        {
            var lcm = new Monom();
            //Количество переменных в мономе
            if (a.Powers.Count > b.Powers.Count)
                b.CompleteMonom(b);
            else if (a.Powers.Count < b.Powers.Count)
                a.CompleteMonom(a);
            if (a.Powers.Count == b.Powers.Count)
            {
                for (int i = 0; i < a.Powers.Count; i++)
                {
                    if (a.Powers[i] > b.Powers[i])
                        lcm.Powers.Add(a.Powers[i]);
                    else
                        lcm.Powers.Add(b.Powers[i]);
                }
            }
            return lcm;
        }

        ///<summary> 
        ///Дополнение монома переменными 0-й степени 
        ///</summary>
        ///<param name="compared">  Дополнение на столько же переменных, сколько у него </param>
        ///<returns> Дополненный моном </returns>
        public Monom CompleteMonom(Monom compared)
        {
            if (Powers.Count < compared.Powers.Count)
            {
                while (Powers.Count < compared.Powers.Count)
                    Powers.Add(0);
            }
            else if (Powers.Count > compared.Powers.Count)
            {
                while (Powers.Count > compared.Powers.Count)
                    compared.Powers.Add(0);
            }

            return this;
        }

        /// <summary>
        /// Проверка мономов на делимость
        /// </summary>
        /// <param name="a">Делимое</param>
        /// <param name="b">Делитель</param>
        /// <returns> да/нет </returns>
        public static bool CanDivide(Monom a, Monom b)
        {
            bool canDivide = true;

            if (b._coef == 0)
                canDivide = false;
            else
            {
                if (a.Powers.Count > b.Powers.Count)
                {
                    b.CompleteMonom(a);
                    if (a.Powers.Where((t, i) => t < b.Powers[i]).Any())
                    {
                        canDivide = false;
                    }
                }
            }

            return canDivide;
        }

        /// <summary>
        /// Строковое представление монома
        /// </summary>
        /// <returns> Строковое представление </returns>
        public override string ToString()
        {
            string toShow = "";

            for (int i = 0; i < Powers.Count; i++)
            {
                if (Powers[i] != 0)
                    toShow += Math.Abs(Powers[i]) > 1
                        ? "*x" + (i + 1) + "^" + (Powers[i])
                        : "*x" + (i + 1);
            }
            return $"<{toShow.Insert(0, Coef.ToString("0.###;-0.###;0"))}>";
        }

        /// <summary>
        /// Проверка мономов на подобие
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns> подобны/нет </returns>
        public static bool AreEqual(Monom a, Monom b)
        {
            bool areEqual = false;
            if (a.Powers.Count == b.Powers.Count)
            {
                if (a.Powers.SequenceEqual(b.Powers))
                {
                    areEqual = true;
                }
            }
            return areEqual;
        }

        #region Операторы унарные/бинарные

        public static Monom operator +(Monom a, Monom b)
        {
            Monom sumMonom = null;
            if (AreEqual(a, b))
            {
                sumMonom = new Monom(a._coef + b._coef, a.Powers);
            }
            return sumMonom;
        }

        public static Monom operator -(Monom a, Monom b)
        {
            Monom subMonom = null;
            if (AreEqual(a, b))
            {
                subMonom = new Monom(a._coef - b._coef, a.Powers);
            }
            return subMonom;
        }

        public static Monom operator -(Monom m)
        {
            Monom tempMonom = m;
            tempMonom._coef *= -1;
            return tempMonom;
        }

        public static Monom operator *(Monom a, Monom b)
        {
            var multMonom = new Monom();
            multMonom._coef = a._coef * b._coef;

            if (a.Powers.Count > b.Powers.Count) b.CompleteMonom(a);
            else if (a.Powers.Count < b.Powers.Count) a.CompleteMonom(b);

            for (int i = 0; i < a.Powers.Count; i++)
            {
                multMonom.Powers.Add(a.Powers[i] + b.Powers[i]);
            }
            return multMonom;
        }

        public static Monom operator /(Monom a, Monom b)
        {
            Monom divMonom = new Monom();

            if (CanDivide(a, b))
            {
                divMonom._coef = a._coef / b._coef;
                for (int i = 0; i < a.Powers.Count; i++)
                {
                    divMonom.Powers.Add(a.Powers[i] - b.Powers[i]);
                }
            }
            return divMonom;
        }

        #endregion
    }

}
