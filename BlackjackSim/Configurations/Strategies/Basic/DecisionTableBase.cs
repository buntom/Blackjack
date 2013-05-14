using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BlackjackSim.Configurations.Strategies.Basic
{
    public abstract class DecisionTableBase
    {
        [XmlAttribute]
        public bool DealerStandsSoft17 { get; set; }

        public string Matrix { get; set; }

        [XmlIgnore]
        private int[,] matrixArray;

        [XmlIgnore]
        public int[,] MatrixAsArray
        {
            get
            {
                if (matrixArray == null)
                {
                    matrixArray = Support.MatrixStringToArray(Matrix);
                    Support.VerifyDimensions(matrixArray, ExpectedRows, ExpectedColumns, Description);
                }

                return matrixArray;
            }
        }

        private readonly int ExpectedRows;
        private readonly int ExpectedColumns;
        private readonly string Description;

        public DecisionTableBase(int expectedRows, int expectedColumns, string description)
        {
            ExpectedRows = expectedRows;
            ExpectedColumns = expectedColumns;
            Description = description;
        }
    }
}
