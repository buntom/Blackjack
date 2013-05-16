using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Diagnostics.Logging;

namespace BlackjackSim.Configurations.Strategies
{
    public class Support
    {
        public static T[,] MatrixStringToArray<T>(string stringMatrix, Func<string, T> convertor)
        {
            const char rowSeparator = ';';
            const char columnSeparator = ',';

            var lines = stringMatrix.Split(rowSeparator);

            int rowCount = lines.Length;

            if (rowCount == 0)
            {
                throw new ArgumentException("Matrix has no rows.");
            }

            int columnCount = lines.First().Split(columnSeparator).Length;

            var matrixArray = new T[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                var row = lines[i];

                var items = row.Split(columnSeparator);

                var rowColumnCount = items.Length;

                if (rowColumnCount != columnCount)
                {
                    var errorMessage = string.Format(
                        "Failed to parse {0}. row - incorrect number of columns. Expected: {1}, actual: {2}. Row: {3}",
                        i, columnCount, rowColumnCount, row);

                    throw new ArgumentException(errorMessage);
                }

                for (int j = 0; j < items.Length; j++)
                {
                    try
                    {
                        matrixArray[i, j] = convertor(items[j]);
                    }
                    catch (ArgumentException exception)
                    {
                        TraceWrapper.LogException(exception);

                        var errorMessage = string.Format(
                            "Failed to parse element of matrix:  row: {0}, column: {1}, given value: {2}",
                            i, j, items[j]);

                        throw new ArgumentException(errorMessage);
                    }
                }
            }

            return matrixArray;
        }

        public static void VerifyDimensions<T>(T[,] matrix, int expectedRows, int expectedColumns, string matrixDescription)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            if (rows != expectedRows || columns != expectedColumns)
            {
                var errorMessage = string.Format("Wrong dimension of {0}: expected {1} x {2}, given {3} x {4}",
                    matrixDescription, expectedRows, expectedColumns, rows, columns);

                throw new Exception(errorMessage);
            }
        }
    }
}
