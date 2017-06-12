using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.Expression;
using Symbolic.Model.Polynomial;

namespace Symbolic.Model.Parser
{
    static class PolynomParser
    {
        public static Polynom Parse(string polyString)
        {
            var tmp = polyString.Replace("-", "+-").Split('+').ToList();
            var terms = RemoveSpaces(tmp);

            var monoms = BuildMonoms(terms);
            var poly = LexOrder.CreateOrderedPolynom(new Polynomial.Polynom(monoms));

            return poly;
        }

        public static List<string> RemoveSpaces(List<string> terms)
        {
            while (terms.Contains(""))
            {
                int index = terms.FindIndex(x => x == "" || x == " ");
                terms.RemoveAt(index);
            }

            return new List<string>(terms);
        }

        public static List<Monom> BuildMonoms(List<string> terms)
        {
            List<Monom> monoms = new List<Monom>();

            foreach (var term in terms)
            {
                Monom m = new Monom();
                var monomTerms = term.Split('*');
                
                foreach (var item in monomTerms)
                {
                    var newItem = ExceptChars(item, new List<char>() { ' ' });
                    if (newItem.All(x=>!char.IsLetter(x)))
                    {
                        m.Coef = Convert.ToDouble(newItem);
                    }
                    else
                    {
                        if (!newItem.Contains("^"))
                        {
                            m.Powers.Add(new Tuple<string, int>(newItem, 1));
                        }
                        else
                        {
                            var variableTerms = newItem.Split('^');
                            m.Powers.Add(new Tuple<string, int>(variableTerms[0], Convert.ToInt32(variableTerms[1])));
                        }
                    }
                }

                monoms.Add(m);
            }

            return monoms;
        }

        public static string ExceptChars(this string str, IEnumerable<char> toExclude)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (!toExclude.Contains(c))
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
