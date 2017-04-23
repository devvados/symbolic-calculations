using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Polynomial
{
    class PolynomSystem
    {
        public List<Polynom> System;

        #region Конструкторы

        public PolynomSystem()
        {
            System = new List<Polynom>();
        }

        public PolynomSystem(List<Polynom> polylist)
        {
            System = new List<Polynom>(polylist);
        }

        #endregion

        #region Свойства

        public Polynom this[int index]
        {
            get => System[index];
            set => System[index] = value;
        }

        #endregion

        public PolynomSystem GroebnerBasis()
        {
            //LOGIC

            return new PolynomSystem(System);
        }
    }
}
