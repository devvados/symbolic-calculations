using Antlr4.Runtime;
using MathNet.Symbolics;
using Symbolic.Model.Parser;
using Symbolic.Model.Polynomial;
using Symbolic.Parser;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Symbolic.Pages
{
    /// <summary>
    /// Interaction logic for IntegralPage.xaml
    /// </summary>
    public partial class IntegralPage : Page
    {
        public IntegralPage()
        {
            InitializeComponent();
        }

        private void BCalcPolynomIntegral_Click(object sender, RoutedEventArgs e)
        {
            //Парсинг числителя и знаменателя
            var numerator = Infix.ParseOrThrow(TbNumerator.Text);
            var denominator = Infix.ParseOrThrow(TbDenominator.Text);
            //Упрощение числителя и знаменателя по отдельности
            var simplifiedNumerator = Trigonometric.Simplify(numerator);
            var simplifiedDenominator = Trigonometric.Simplify(denominator);

            //Строим  и упрощаем рациональную функцию
            RationalFunction rf = new RationalFunction();
            rf.Numerator = PolynomParser.Parse(Infix.Format(simplifiedNumerator));
            rf.Denominator = PolynomParser.Parse(Infix.Format(simplifiedDenominator));
            List<Monom> division;
            var f = rf.Simplify(out division).ToString();

            var sourceRationalFunction = LaTeX.Format(Infix.ParseOrThrow(rf.ToString()));

            var indefiniteIntegral = rf.Integrate();
            var parsedFunction = (new CalculatorVisitor().Visit(
                new CalculatorParser(new CommonTokenStream(
                    new CalculatorLexer(new AntlrInputStream(indefiniteIntegral.ToString())))).prog()));
            var texFormula = LaTeX.Format(Infix.ParseOrThrow(parsedFunction.ToString()));

            TBIntFormula.Text = @"\int f = " + texFormula;
        }
    }
}
