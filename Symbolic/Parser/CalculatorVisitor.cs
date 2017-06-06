using Symbolic.Model.Base;
using Symbolic.Model.Operation;
using Symbolic.Model.Template;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace Symbolic.Parser
{
    public class CalculatorVisitor : CalculatorBaseVisitor<Function>
    {
        private static readonly Dictionary<int, Function> FunctionMap = new Dictionary<int, Function>
        {
            { CalculatorParser.SIN, new Sinus(new Identity() - 1) },
            { CalculatorParser.COS, new Cosinus() },
            { CalculatorParser.SQRT, new Sqrt() },
            { CalculatorParser.LN, new Logarithm() },
            { CalculatorParser.X, new Identity() },
            { CalculatorParser.EXP, new Exponenta() }
        };

        public override Function VisitPow(CalculatorParser.PowContext context)
        {
            return new Power(Visit(context.expr(0)), Visit(context.expr(1)));
        }

        public override Function VisitMulDiv(CalculatorParser.MulDivContext context)
        {
            Function res = null;

            if (context.op.Text == "*")
                res = Visit(context.expr(0)) * Visit(context.expr(1));
            if (context.op.Text == "/")
                res =  Visit(context.expr(0)) / Visit(context.expr(1));

            return res;
        }

        public override Function VisitAddSub(CalculatorParser.AddSubContext context)
        {
            Function res = null;

            if (context.op.Text == "+")
                res = Visit(context.expr(0)) + Visit(context.expr(1));
            if (context.op.Text == "-")
                res = Visit(context.expr(0)) - Visit(context.expr(1));

            return res;
        }

        public override Function VisitVal(CalculatorParser.ValContext context)
        {
            return new Constant(Convert.ToDouble(context.VAL().GetText(), CultureInfo.InvariantCulture.NumberFormat));
        }

        public override Function VisitFunction(CalculatorParser.FunctionContext context)
        {
            Function res = null;

            if (FunctionMap[context.fun.Type] is Identity)
                res = new Identity();
            if (FunctionMap[context.fun.Type] is Sinus)
                res = new Sinus(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Cosinus)
                res = new Cosinus(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Sqrt)
                res = new Sqrt(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Logarithm)
                res = new Logarithm(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Exponenta)
                res = new Exponenta();
            

            return res;
        }

        public override Function VisitParens(CalculatorParser.ParensContext context)
        {
            return (Visit(context.expr()));
        }

        public override Function VisitIdentity(CalculatorParser.IdentityContext context)
        {
            return new Identity();
        }

        public override Function VisitMinus(CalculatorParser.MinusContext context)
        {
            return (-1) * Visit(context.expr());
        }
        public override Function VisitMultNoSign(CalculatorParser.MultNoSignContext context)
        {
            return Visit(context.expr(0)) * Visit(context.expr(1));
        }
    }
}
