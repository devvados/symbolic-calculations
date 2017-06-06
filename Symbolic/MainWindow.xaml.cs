using System.Collections.Generic;
using Symbolic.Model.Polynomial;
using Symbolic.Model.Template;
using static Symbolic.Model.Template.Funcs;
using MahApps.Metro.Controls;
using Symbolic.Model.Base;
using Symbolic.Parser;
using Antlr4.Runtime;
using WolframAlphaNET;
using WolframAlphaNET.Objects;
using System.Threading;
using System;
using System.Windows;
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.Expression;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using static MathNet.Symbolics.Approximation;

namespace Symbolic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model.Base.Function f;
        private bool isProcessRunning = false;

        DataTable _myDataTable;

        public int[,] Data2D { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            TbFirst.Focus();

            TestRationalFunctions();

            BuildMatrix(3);
        }


        void BuildMatrix(int size)
        {
            _myDataTable = new DataTable();

            var Data2D = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Data2D[i, j] = 1;
                }
            }

            // create columns
            for (int i = 0; i < size; i++)
            {
                DataColumn c = new DataColumn { ColumnName = (i + 1).ToString() };
                _myDataTable.Columns.Add(c);
            }

            for (int j = 0; j < size; j++)
            {
                // create a DataRow using .NewRow()
                DataRow row = _myDataTable.NewRow();

                // iterate over all columns to fill the row
                for (int i = 0; i < size; i++)
                {
                    row[i] = Data2D[i, j];
                }

                // add the current row to the DataTable
                _myDataTable.Rows.Add(row);
            }
            DGMatrix.DataContext = _myDataTable;
            FitToContent();
        }

        private void GetSymbols()
        {
            List<string> variables = new List<string>();

            var input = TbFirst.Text;
            var symbols = input.Split('+', '-', '*', '/', '(', ')');

            foreach (var symbol in symbols)
            {
                if (symbol.Contains("x"))
                {
                    if (!variables.Contains(symbol))
                    {
                        variables.Add(symbol);
                    }
                }
            }
        }

        private void FitToContent()
        {
            // where dg is my data grid's name...
            foreach (DataGridColumn column in DGMatrix.Columns)
            {
                //if you want to size ur column as per both header and cell content
                column.Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto);    
            }
        }

        /// <summary>
        /// Интегрирование
        /// </summary>
        private void TestRationalFunctions()
        {
            //Cоздаем рациональную функцию
            var numerator = new Polynom(new List<Monom>
            {
                new Monom(2, new List<Tuple<string, int>> {
                        new Tuple<string, int>("x", 1),
                        //new Tuple<string, int>("c", 1),
                        new Tuple<string, int> ("x", 2)
                }),
                new Monom(3, new List<Tuple<string, int>> {
                        new Tuple<string, int>("x", 2),
                        //new Tuple<string, int>("x", 1),
                        //new Tuple<string, int>("c", 2)
                })
            });

            var denominator = new Polynom(new List<Monom>
            {
                new Monom(1, new List<Tuple<string, int>> {
                        new Tuple<string, int>("x", 2),
                        //new Tuple<string, int> ("x", 1)
                }),
                new Monom(1, new List<Tuple<string, int>> {
                    new Tuple<string, int>("x", 1),
                    //new Tuple<string, int>("x", 2)
                })
            });

            numerator = LexOrder.CreateOrderedPolynom(numerator);
            denominator = LexOrder.CreateOrderedPolynom(denominator);

            var rationalFunction = new RationalFunction(numerator, denominator);

            foreach (var m in numerator.Monoms)
            {
                m.OrderVariables();
            }
            foreach (var m in denominator.Monoms)
            {
                m.OrderVariables();
            }

            ////Упрощаем числитель и знаменатель по отдельности и выводим
            TbNumerator.Text = numerator.SimplifyPolynom().ToString();
            TbDenominator.Text = denominator.SimplifyPolynom().ToString();

            ////Упрощаем функцию делением (если возможно)
            //var division = new List<Monom>();
            //var simplifiedRationalFunction = rationalFunction.Simplify(out division);
        }

        #region Работа с математическими выражениями

        /// <summary>
        /// Разбор математического выражения
        /// </summary>
        public Model.Base.Function ParseFormula()
        {
            var input = TbFirst.Text;

            //var func = Infix.ParseOrThrow(input);
            //var funcOrdered = Trigonometric.Expand(func);
            //var funcOrderedString = Infix.Format(func);

            var parsedFunction = (new CalculatorVisitor().Visit(
                new CalculatorParser(new CommonTokenStream(
                    new CalculatorLexer(new AntlrInputStream(input)))).prog()));

            return parsedFunction;
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

        #endregion

        #region Обработка кнопок

        private void BCalcDerivative_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GetSymbols();

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
                    
                    var sourceFormula = Infix.ParseOrUndefined(f.ToString());

                    //вычисление
                    var texDer = SimplifyFunctionsByMathNET(f);

                    ShowProcessState(false);
 
                    TBDerivFormula.BeginInvoke(
                        new Action(() =>
                        {
                            TBDerivFormula.Text = @"f' = \left(" + LaTeX.Format(sourceFormula) + 
                                                  @"\right)' = " + LaTeX.Format(texDer);
                        }
                    ));
                }
            ));

            simplifyThread.Start();
        }

        #endregion

        #region Отображение многопоточности

        /// <summary>
        /// Обозначение прогресса операции
        /// </summary>
        /// <param name="isIndeterminate"></param>
        public void SetIndeterminate(bool isIndeterminate)
        {
            PBDerivativeLoading.BeginInvoke(
                new Action(() =>
                {
                    if (isIndeterminate)
                    {
                        PBDerivativeLoading.IsIndeterminate = true;
                    }
                    else
                    {
                        PBDerivativeLoading.IsIndeterminate = false;
                    }
                }
            ));
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

        #endregion

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

            ////Выводим результат деления
            //var parsedReminder = Infix.ParseOrThrow(f);
            //var parsedDivision = Infix.ParseOrThrow(new Polynom(division).ToString());
            //var simplifiedReminder = Trigonometric.Simplify(parsedReminder);
            //var simplifiedDivision = Trigonometric.Simplify(parsedDivision);
            //var reminderString = Infix.Format(simplifiedReminder);
            //var divisionString = Infix.Format(simplifiedDivision);

            var sourceRationalFunction = LaTeX.Format(Infix.ParseOrThrow(rf.ToString()));
            //var simplifiedRationalFunction = LaTeX.Format(Trigonometric.Simplify(Infix.ParseOrThrow(reminderString)));
            //var divisionFromSimplify = LaTeX.Format(Infix.ParseOrThrow(divisionString));

            //var formula = @"\int f = " + @"\int \left(" + sourceRationalFunction + @"\right)" + " = " + @"\int \left(" + divisionFromSimplify + @"\right)" + @"+ \int \left(" + simplifiedRationalFunction + @"\right)";
            //var form = Ln() + (-0.5) * Ln((Id ^ 2) + 1);
            //var fs = Infix.ParseOrThrow(form.ToString());
            //var fsf = LaTeX.Format(fs);

            //var formula = @"\int fdx = " + @"\int \left(" + sourceRationalFunction + @"\right)dx" + " = " + fsf + " + C";
            //TBIntFormula.Text = formula;

            var indefiniteIntegral = rf.Integrate();
            var parsedFunction = (new CalculatorVisitor().Visit(
                new CalculatorParser(new CommonTokenStream(
                    new CalculatorLexer(new AntlrInputStream(indefiniteIntegral.ToString())))).prog()));
            var texFormula = LaTeX.Format(Infix.ParseOrThrow(parsedFunction.ToString()));

            TBIntFormula.Text = @"\int f = " + texFormula;


        }

        private void BCreateMatrix_Click(object sender, RoutedEventArgs e)
        {
            int dim = Convert.ToInt32(TBMatrixDimension.Text);

            BuildMatrix(dim);
        }

        private void BMatrixDet_Click(object sender, RoutedEventArgs e)
        {
            var tableEnumerable = ((DataView)DGMatrix.ItemsSource).ToTable();
            var tableArray = tableEnumerable.AsEnumerable().Select(row => row.ItemArray).ToArray();

            var d = To2D(tableArray);

            Matrix<double> A = DenseMatrix.OfArray(d);
            var det = A.Determinant();

            MessageBox.Show(det.ToString());
        }

        static double[,] To2D(object[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new double[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = Convert.ToDouble(source[i][j]);

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }
    }
}
