using Symbolic.Model.Template;

namespace Symbolic.Model.Base
{
    public class Constant : Function
    {
        private readonly double _value;

        public Constant(double val)
        {
            _value = val;
        }

        public override double Calc(double val)
        {
            return _value;
        }

        public override Function Derivative()
        {
            return Funcs.Zero;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
