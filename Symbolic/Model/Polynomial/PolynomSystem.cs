using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Polynomial
{
    class PolynomSystem
    {
        public List<Polynom> system;

        #region Конструкторы

        public PolynomSystem()
        {
            system = new List<Polynom>();
        }

        public PolynomSystem(List<Polynom> polylist)
        {
            system = new List<Polynom>(polylist);
        }

        #endregion

        #region Свойства

        public Polynom this[int index]
        {
            get
            {
                return system[index];
            }
            set
            {
                system[index] = value;
            }
        }

        #endregion

        public PolynomSystem GroebnerBasis()
        {
            //LOGIC

            return new PolynomSystem(system);
        }
    }
}
