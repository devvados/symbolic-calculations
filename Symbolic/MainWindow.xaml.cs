using System;
using System.Collections.Generic;
using Symbolic.Model.Polynomial;
using Symbolic.Model.Template;
using MahApps.Metro.Controls;

namespace Symbolic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            TestDifferentialCalculus();

            TestRationalFunctions();
        }

        /// <summary>
        /// Интегрирование
        /// </summary>
        private void TestRationalFunctions()
        {
            var numerator = new Polynom(new List<Monom>
            {
                new Monom(4, new List<int> { 4 }),
                new Monom(8, new List<int> { 3 }),
                new Monom(0, new List<int> { 2 }),
                new Monom(-3, new List<int>{ 1 }),
                new Monom(-3, new List<int>{ 0 })
            });

            var denominator = new Polynom(new List<Monom>
            {
                new Monom(1, new List<int>{3}),
                new Monom(2, new List<int>{2 }),
                new Monom(1, new List<int>{1 }),
                new Monom(0)
            });

            var rationalFunction = new RationalFunction(numerator, denominator);

            TbNumerator.Text = numerator.SimplifyPolynom().ToString();
            TbDenominator.Text = denominator.SimplifyPolynom().ToString();

            var division = new List<Monom>();
            var simplifiedRationalFunction = rationalFunction.Simplify(out division);

            TbIntegralNumerator.Text = (simplifiedRationalFunction as RationalFunction)?.Numerator.SimplifyPolynom().ToString();
            TbIntegralDenominator.Text = (simplifiedRationalFunction as RationalFunction)?.Denominator.SimplifyPolynom().ToString();
        }

        /// <summary>
        /// Дифференцирование
        /// </summary>
        public void TestDifferentialCalculus()
        {
            var f = 2 * Funcs.Exp(Funcs.Ln());
            TbFirst.Text = $"f = {f}";
            TbSecond.Text = $"f' = {f.Derivative()}";
        }
    }
}
