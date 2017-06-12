using Symbolic.Model.Base;
using static Symbolic.Model.Template.Funcs;
using Symbolic.Model.Operation;
using Symbolic.Model.Template;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Symbolic.Model.Template.Trigonometric;
using Symbolic.Model.Template.InverseTrig;
using Symbolic.Model.Template.Hyperbolic;

namespace Symbolic.Parser
{
    public class CalculatorVisitor : CalculatorBaseVisitor<Function>
    {
        private static readonly Dictionary<int, Function> FunctionMap = new Dictionary<int, Function>
        {
            { CalculatorParser.SIN, Sin() },
            { CalculatorParser.COS, Cos() },
            { CalculatorParser.TAN, Tan() },
            { CalculatorParser.COT, Cot() },
            { CalculatorParser.ARCSIN, Asin() },
            { CalculatorParser.ARCCOS, Acos() },
            { CalculatorParser.ARCTAN, Atan() },
            { CalculatorParser.ARCCOT, Acot() },
            { CalculatorParser.SQRT, Sq() },
            { CalculatorParser.LN, Ln() },
            { CalculatorParser.EXP, Exp() },
            { CalculatorParser.SH, Sh() },
            { CalculatorParser.CH, Ch() },
            { CalculatorParser.TH, Tgh() },
            { CalculatorParser.CTH, Cth() },
        };

        /// <summary>
        /// Power operator
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Function VisitPow(CalculatorParser.PowContext context)
        {
            return new Power(Visit(context.expr(0)), Visit(context.expr(1)));
        }

        /// <summary>
        /// Multiply or Divide operator
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Function VisitMulDiv(CalculatorParser.MulDivContext context)
        {
            Function res = null;

            if (context.op.Text == "*")
                res = Visit(context.expr(0)) * Visit(context.expr(1));
            if (context.op.Text == "/")
                res =  Visit(context.expr(0)) / Visit(context.expr(1));

            return res;
        }

        /// <summary>
        /// Addition or Substraction operator
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Function VisitAddSub(CalculatorParser.AddSubContext context)
        {
            Function res = null;

            if (context.op.Text == "+")
                res = Visit(context.expr(0)) + Visit(context.expr(1));
            if (context.op.Text == "-")
                res = Visit(context.expr(0)) - Visit(context.expr(1));

            return res;
        }

        /// <summary>
        /// Value expression
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Function VisitVal(CalculatorParser.ValContext context)
        {
            return new Constant(Convert.ToDouble(context.VAL().GetText(), CultureInfo.InvariantCulture.NumberFormat));
        }

        /// <summary>
        /// Function expression
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Function VisitFunction(CalculatorParser.FunctionContext context)
        {
            Function res = null;

            //Trigonometric functions
            if (FunctionMap[context.fun.Type] is Sinus)
                res = new Sinus(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Cosinus)
                res = new Cosinus(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Tangens)
                res = new Tangens(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Cotangens)
                res = new Cotangens(Visit(context.expr()));

            //Elementary functions
            if (FunctionMap[context.fun.Type] is Sqrt)
                res = new Sqrt(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Logarithm)
                res = new Logarithm(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Exponenta)
                res = new Exponenta(Visit(context.expr()));

            //Inverse Trigonometric functions
            if (FunctionMap[context.fun.Type] is Arcsinus)
                res = new Arcsinus(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Arccosinus)
                res = new Arccosinus(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Arctangens)
                res = new Arctangens(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is Arccotangens)
                res = new Arccotangens(Visit(context.expr()));

            //Hyperbolic functions
            if (FunctionMap[context.fun.Type] is HypSinus)
                res = new HypSinus(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is HypCosinus)
                res = new HypCosinus(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is HypTangens)
                res = new HypTangens(Visit(context.expr()));
            if (FunctionMap[context.fun.Type] is HypCotangens)
                res = new HypCotangens(Visit(context.expr()));

            return res;
        }

        /// <summary>
        /// Parens
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Function VisitParens(CalculatorParser.ParensContext context)
        {
            return (Visit(context.expr()));
        }

        /// <summary>
        /// Identity (variable)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Function VisitIdentity(CalculatorParser.IdentityContext context)
        {
            return new Identity();
        }

        /// <summary>
        /// Unary Minus operator
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Function VisitMinus(CalculatorParser.MinusContext context)
        {
            return (-1) * Visit(context.expr());
        }

        /// <summary>
        /// Multiply with no operator
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Function VisitMultNoSign(CalculatorParser.MultNoSignContext context)
        {
            return Visit(context.expr(0)) * Visit(context.expr(1));
        }
    }
}
