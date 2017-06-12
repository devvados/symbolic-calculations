using Antlr4.Runtime;
using MathNet.Symbolics;
using Symbolic.Model.Parser;
using Symbolic.Model.Polynomial;
using Symbolic.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolframAlphaNET.Objects;

namespace Symbolic.Model.Base
{
    class Matrix
    {
        List<List<Polynom>> mat;

        public Matrix(List<List<Polynom>> l)
        {
            mat = new List<List<Polynom>>();
            foreach (var item in l)
            {
                mat.Add(Extensions.Clone(item));
            }
        }

        public Polynom Determinant()
        {
            WolframAlphaNET.WolframAlpha wolfram = new WolframAlphaNET.WolframAlpha("RAG9YE-E5PVQUEEKT");
            var query = wolfram.Query(this.ToWolframString());
            Polynom poly = new Polynom();

            if (query != null)
            {
                var s = query.Pods[1].SubPods[0].Plaintext.ToString();
                var parsedFunction = (new CalculatorVisitor().Visit(
                new CalculatorParser(new CommonTokenStream(
                    new CalculatorLexer(new AntlrInputStream(s)))).prog()));

                var temp = Infix.ParseOrThrow(parsedFunction.ToString());
                poly = PolynomParser.Parse(Infix.Format(Trigonometric.Simplify(temp)));
            }

            return LexOrder.CreateOrderedPolynom(poly);
        }

        public string ToWolframString()
        {
            string mainQuery = "";
            StringBuilder subSub = new StringBuilder();

            //subSub.Append("{");
            foreach (var item in mat)
            {
                subSub.Append("{");
                foreach (var subItem in item)
                {
                    if (item.IndexOf(subItem) != item.Count - 1)
                        subSub.Append(subItem.ToString().Replace("c","x") + ",");
                    else
                    {
                        subSub.Append(subItem.ToString().Replace("c", "x"));
                    }
                }
                if (mat.IndexOf(item) != mat.Count - 1)
                    subSub.Append("},");
                else 
                    subSub.Append("}");
            }
            //subSub.Append("}");

            mainQuery = "determinant of {" + subSub + "}";

            return mainQuery;
        }
    }
}
