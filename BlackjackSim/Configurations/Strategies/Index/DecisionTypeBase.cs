using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackjackSim.Configurations.Strategies.Index
{
    public struct DecisionTypeBase
    {
        public readonly int? indexNumber;        
        public readonly bool? fixedDecision;
        public readonly bool? revertIndexDecision;

        public DecisionTypeBase(int? indexNumber, bool? fixedDecision, bool? revertIndexDecision)
        {
            if ((indexNumber == null && fixedDecision == null) ||
                (indexNumber != null && fixedDecision != null) ||
                (indexNumber != null && revertIndexDecision == null))
            {
                throw new ArgumentException("Wrong definition of Index Strategy decision type!");
            }

            this.indexNumber = indexNumber;            
            this.fixedDecision = fixedDecision;
            this.revertIndexDecision = revertIndexDecision;
        }

        public int? IndexNumber { get { return indexNumber; } }
        public bool? FixedDecision { get { return fixedDecision; } }
        public bool? RevertIndexDecision { get { return revertIndexDecision; } }

        public static DecisionTypeBase ConvertFromString(string codeString)
        {
            int? indexNumber = null;            
            bool? fixedDecision = null;
            bool? revertIndexDecision = null;
            
            var codeStringEdit = codeString.Trim().ToUpper();
            if (codeStringEdit == "A")
            {
                fixedDecision = true;
            }
            else if (codeStringEdit == "N")
            {
                fixedDecision = false;
            }
            else if (codeStringEdit.Contains("*"))
            {
                revertIndexDecision = true;
                int indexNumberStarts = codeStringEdit.IndexOf("*");
                var indexNumberString = codeStringEdit.Substring(indexNumberStarts + 1);
                int indexNumberAux;
                var success = int.TryParse(indexNumberString, out indexNumberAux);
                if (!success)
                {
                    throw new ArgumentException(string.Format("Wrong decision code: {0}", codeString));
                }
                indexNumber = indexNumberAux;
            }
            else
            {
                revertIndexDecision = false;
                int indexNumberAux;
                var success = int.TryParse(codeStringEdit, out indexNumberAux);
                if (!success)
                {
                    throw new ArgumentException(string.Format("Wrong decision code: {0}", codeString));
                }
                indexNumber = indexNumberAux;
            }

            var decision = new DecisionTypeBase(indexNumber, fixedDecision, revertIndexDecision);
            return decision;
        }
    }
}
