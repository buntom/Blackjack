using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BlackjackSim.Configurations.Strategies
{
    public abstract class DecisionTableBase<T>
    {
        [XmlAttribute]
        public bool DealerStandsSoft17 { get; set; }

        public string Matrix { get; set; }

        [XmlIgnore]
        private T[,] matrixArray;

        [XmlIgnore]
        public T[,] MatrixAsArray
        {
            get
            {
                if (matrixArray == null)
                {
                    matrixArray = Support.MatrixStringToArray<T>(Matrix, Convertor);
                    Support.VerifyDimensions<T>(matrixArray, ExpectedRows, ExpectedColumns, Description);
                }

                return matrixArray;
            }
        }

        private readonly int ExpectedRows;
        private readonly int ExpectedColumns;
        private readonly string Description;
        private readonly Func<string, T> Convertor;

        public DecisionTableBase(int expectedRows, int expectedColumns, string description, Func<string, T> convertor)
        {
            ExpectedRows = expectedRows;
            ExpectedColumns = expectedColumns;
            Description = description;
            Convertor = convertor;
        }
    }
}
