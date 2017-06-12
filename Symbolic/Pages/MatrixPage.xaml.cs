using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Symbolic.Model.Polynomial;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Symbolic.Pages
{
    /// <summary>
    /// Interaction logic for MatrixPage.xaml
    /// </summary>
    public partial class MatrixPage : Page
    {
        public MatrixPage()
        {
            InitializeComponent();            
        }

        static double[,] To2D(object[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                // throws InvalidOperationException if source is not rectangular
                int SecondDim = source.GroupBy(row => row.Length).Single().Key;

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

        /// <summary>
        /// Определитель матрицы
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        static double FindDeterminant(double[,] m)
        {
            Matrix<double> A = DenseMatrix.OfArray(m);
            var det = A.Determinant();

            return det;
        }

        /// <summary>
        /// Ранг матрицы
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        static double FindRank(double[,] m)
        {
            Matrix<double> A = DenseMatrix.OfArray(m);
            var rank = A.Rank();

            return rank;
        }

        /// <summary>
        /// Ядро матрицы
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        static string FindKernel(double[,] m)
        {
            StringBuilder sb = new StringBuilder();

            Matrix<double> A = DenseMatrix.OfArray(m);
            var ker = A.Kernel();

            if (ker.Count() > 0)
            {
                sb.Append("{");
                foreach (var item in ker)
                {
                    sb.Append("(");
                    foreach (var value in item)
                    {
                        sb.Append(" " + value.ToString() + " ");
                    }
                    sb.Append(")");
                }
                sb.Append("}");
            }

            var s = sb.Replace("( ", "(").Replace(" )", ")").Replace(")(", "),(").ToString();

            return s;
        }

        #region Обработка кнопок

        private void BMatrixDet_Click(object sender, RoutedEventArgs e)
        {
            Button pressedButton = sender as Button;
            DataTable tableEnumerable = new DataTable();
            
            if (pressedButton!= null)
            {
                if(pressedButton.Tag.ToString() == "A")
                    tableEnumerable = ((DataView)DGMatrixA.DGMatrix.ItemsSource).ToTable();
                if (pressedButton.Tag.ToString() == "B")
                    tableEnumerable = ((DataView)DGMatrixB.DGMatrix.ItemsSource).ToTable();
            }
         
            var tableArray = tableEnumerable.AsEnumerable().Select(row => row.ItemArray).ToArray();

            var d = To2D(tableArray);

            var det = FindDeterminant(d);

            MessageBox.Show(det.ToString());
        }
        
        private void BMatrixRank_Click(object sender, RoutedEventArgs e)
        {
            Button pressedButton = sender as Button;
            DataTable tableEnumerable = new DataTable();

            if (pressedButton != null)
            {
                if (pressedButton.Tag.ToString() == "A")
                    tableEnumerable = ((DataView)DGMatrixA.DGMatrix.ItemsSource).ToTable();
                if (pressedButton.Tag.ToString() == "B")
                    tableEnumerable = ((DataView)DGMatrixB.DGMatrix.ItemsSource).ToTable();
            }

            var tableArray = tableEnumerable.AsEnumerable().Select(row => row.ItemArray).ToArray();

            var d = To2D(tableArray);

            var rank = FindRank(d);

            MessageBox.Show(rank.ToString());
        }

        private void BMatrixKernel_Click(object sender, RoutedEventArgs e)
        {
            Button pressedButton = sender as Button;
            DataTable tableEnumerable = new DataTable();

            if (pressedButton != null)
            {
                if (pressedButton.Tag.ToString() == "A")
                    tableEnumerable = ((DataView)DGMatrixA.DGMatrix.ItemsSource).ToTable();
                if (pressedButton.Tag.ToString() == "B")
                    tableEnumerable = ((DataView)DGMatrixB.DGMatrix.ItemsSource).ToTable();
            }

            var tableArray = tableEnumerable.AsEnumerable().Select(row => row.ItemArray).ToArray();

            var d = To2D(tableArray);

            string ker = FindKernel(d);

            MessageBox.Show(ker);
        }

        private void BSum_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BDif_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BMult_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }
}
