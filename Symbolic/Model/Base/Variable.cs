using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Base
{
    class Variable
    {
        private int power;
        private string varString;

        public string VarString { get { return varString; } set { varString = value; } }
        public int Power { get { return power; } set { power = value; } }

        public Variable(string s, double p = 1)
        {
            VarString = s;
        }

        
    }
}
