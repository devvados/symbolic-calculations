using Antlr4.Runtime;
using MathNet.Symbolics;
using Symbolic.Parser;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Expr = MathNet.Symbolics.Expression;

namespace Symbolic.Pages
{
    /// <summary>
    /// Interaction logic for DerivativePage.xaml
    /// </summary>
    public partial class DerivativePage : Page
    {
        Model.Base.Function f;
        private bool isProcessRunning = false;

        public DerivativePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Разбор математического выражения
        /// </summary>
        public Model.Base.Function ParseFormula()
        {
            var input = TbFirst.Text;

            var parsedFunction = (new CalculatorVisitor().Visit(
                new CalculatorParser(new CommonTokenStream(
                    new CalculatorLexer(new AntlrInputStream(input)))).prog()));

            return parsedFunction;
        }

        /// <summary>
        /// Состояние процесса вычисления
        /// </summary>
        /// <param name="state"></param>
        public void ShowProcessState(bool state)
        {
            isProcessRunning = state;
            SetIndeterminate(state);
        }

        /// <summary>
        /// Обозначение прогресса операции
        /// </summary>
        /// <param name="isIndeterminate"></param>
        public void SetIndeterminate(bool isIndeterminate)
        {
            PBDerivativeLoading.Dispatcher.BeginInvoke(
                new Action(() =>
                {
                    if (isIndeterminate)
                    {
                        PBDerivativeLoading.Visibility = Visibility.Visible;
                        PBDerivativeLoading.IsIndeterminate = true;
                    }
                    else
                    {
                        PBDerivativeLoading.IsIndeterminate = false;
                        PBDerivativeLoading.Visibility = Visibility.Hidden;
                    }
                }
            ));
        }

        /// <summary>
        /// Упрощение математического выражения
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Expr SimplifyFunctionsByMathNET(object input)
        {
            Model.Base.Function func = input as Model.Base.Function;

            if (func is Model.Base.Constant)
            {
                return 0;
            }
            else
            {
                //Упрощаем производную 
                var derivativeFunction = Infix.ParseOrUndefined(func.Derivative().ToString());

                return derivativeFunction;
            }
        }

        private void BCalcDerivative_Click(object sender, RoutedEventArgs e)
        {
            f = ParseFormula();

            //если вычисление уже выполняется на данный момент
            if (isProcessRunning)
            {
                MessageBox.Show("Производная уже считается!");
                return;
            }

            Thread simplifyThread = new Thread(
                new ThreadStart(() =>
                {
                    ShowProcessState(true);

                    //вычисление
                    var texDer = SimplifyFunctionsByMathNET(f);

                    ShowProcessState(false);

                    TBDerivFormula.Dispatcher.BeginInvoke(
                        new Action(() =>
                        {
                            var tex = @"f' = " /*+ @"\left(" + f.ToLatexString() + @"\right)' = "*/ + LaTeX.Format(texDer);
                            TBDerivFormula.Text = tex;
                        }
                    ));
                }
            ));

            simplifyThread.Start();
        }
    }
}
