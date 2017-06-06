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
            var tempPolynom = poly;

            for (var i = 0; i < tempPolynom.Monoms.Count; i++)
            {
                var m = tempPolynom.Monoms[i];
                tempPolynom.Monoms[i] = m.OrderVariables();
            }

            var monomRepeats = tempPolynom.Monoms.ToDictionary(m => m, m => 1);
            foreach (var t in tempPolynom.Monoms)
            {
                foreach (var t1 in tempPolynom.Monoms)
                {
                    if (t.CompareTo(t1) > 0)
                        monomRepeats[t]++;
                }
            }
            monomRepeats = monomRepeats.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            //Составим упорядоченный полином
            var orderedPolynom = new Polynom(monomRepeats.Keys.ToList());

            return orderedPolynom.SimplifyPolynom();
        }
    }
}
