using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Symbolic.UserControls
{
    /// <summary>
    /// Interaction logic for MatrixControl.xaml
    /// </summary>
    public partial class MatrixControl : UserControl
    {
        DataTable _myDataTable;


        public MatrixControl()
        {
            _myDataTable = new DataTable();

            InitializeComponent();
            DGMatrix.ItemsSource = _myDataTable.DefaultView;

            BuildMatrix(3);
        }

        void BuildMatrix(int size)
        {
            var Data2D = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Data2D[i, j] = 0;
                }
            }

            // create columns
            for (int i = 0; i < size; i++)
            {
                DataColumn newColumn = new DataColumn
                {
                    ColumnName = (_myDataTable.Columns.Count + 1).ToString(),
                    DataType = typeof(double),
                };
                _myDataTable.Columns.Add(newColumn);
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
        }

        public void AddDimension()
        {
            //add column
            DataColumn newColumn = new DataColumn
            {
                ColumnName = (_myDataTable.Columns.Count + 1).ToString(),
                DataType = typeof(double),
                DefaultValue = 0
            };
            _myDataTable.Columns.Add(newColumn);

            DataRow row = _myDataTable.NewRow();
            for (int i = 0; i < _myDataTable.Columns.Count; i++)
            {
                row[i] = 0;
            }
            _myDataTable.Rows.Add(row);
        }

        public void RemoveDimension()
        {
            int columnsCount = _myDataTable.Columns.Count, rowsCount = _myDataTable.Rows.Count;
            DataColumn col = _myDataTable.Columns[columnsCount - 1];
            _myDataTable.Columns.Remove(col);

            DataRow row = _myDataTable.Rows[rowsCount - 1];
            _myDataTable.Rows.Remove(row);
        }

        private void BAddDimension_Click(object sender, RoutedEventArgs e)
        {
            DGMatrix.ItemsSource = null;

            AddDimension();
            DGMatrix.ItemsSource = _myDataTable.DefaultView;
        }

        private void BRemoveDimension_Click(object sender, RoutedEventArgs e)
        {
            DGMatrix.ItemsSource = null;

            RemoveDimension();
            DGMatrix.ItemsSource = _myDataTable.DefaultView;
        }
    }
}
