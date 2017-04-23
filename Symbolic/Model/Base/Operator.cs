using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbolic.Model.Base
{
    public abstract class Operator : Function
    {
        protected readonly Function leftFunc;
        protected readonly Function rightFunc;

        //protected Operator(Function a)
        //{
        //    leftFunc = a;
        //}
        protected Operator(Function a, Function b)
        {
            leftFunc = a;
            rightFunc = b;
        }
    }
}
