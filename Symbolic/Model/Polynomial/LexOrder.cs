using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Polynomial
{
    class LexOrder
    {
        /// <summary>
        /// Перестановка мономов в полиноме
        /// </summary>
        /// <param name="poly"> Анализируемый полином </param>
        /// <returns> Готовый полином </returns>
        public static Polynom CreateOrderedPolynom(Polynom poly)
        {
            Polynom tempPolynom = poly, orderedPolynom;

            Dictionary<Monom, int> monomRepeats = tempPolynom.monoms.ToDictionary(m => m, m => 1);
            foreach (Monom t in tempPolynom.monoms)
            {
                foreach (Monom t1 in tempPolynom.monoms)
                {
                    if (t.CompareTo(t1) > 0)
                        monomRepeats[t]++;
                }
            }
            monomRepeats = monomRepeats.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            //Составим упорядоченный полином
            orderedPolynom = new Polynom(monomRepeats.Keys.ToList());

            return orderedPolynom.SimplifyPolynom();
        }
    }
}
