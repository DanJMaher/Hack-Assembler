using System;
using System.Collections.Generic;
using System.Text;

namespace HackAssembler
{
    class Symbol
    {
        //Dictionary which, after construction, hold all pre-defined symbols.  Later, labels and variables are added to this dictionary.
        Dictionary<string, string> symbolDictionary;

        public int variableMemoryLocation { get; private set; }

        public Symbol()
        {
            //Initializes as 16, since that is the first memory location in the Hack language specification that supports a variable.
            variableMemoryLocation = 16;

            //Initializes the dictionary with the pre-defined symbols.
            symbolDictionary = new Dictionary<string, string>()
            {
            {"R0","0" },
            { "R1","1" },
            { "R2","2" },
            { "R3","3" },
            { "R4","4" },
            { "R5","5" },
            { "R6","6" },
            { "R7","7" },
            { "R8","8" },
            { "R9","9" },
            { "R10","10" },
            { "R11","11" },
            { "R12","12" },
            { "R13","13" },
            { "R14","14" },
            { "R15","15" },
            { "SCREEN","16384" },
            { "KBD","24576" },
            { "SP","0" },
            { "LCL","1" },
            { "ARG","2" },
            { "THIS","3" },
            { "THAT","4" },
            };
        }

        //This method removes the parentheses from a label and adds it to the symbol dictionary.
        public void AddLabel(string line, int position)
        {
            line = line.Replace("(", "");
            line = line.Replace(")", "");

            symbolDictionary.Add(line, position.ToString());
        }

        //This method both adds variables to the symbol dictionary AND passes back the corresponding value.
        public string ProcessVariable(string line)
        {
            //Removes '@' symbol.
            line = line.Substring(1);

            //If the value is simply a number, it is passed back.
            if (int.TryParse(line, out int unused))
            {
                return '@' + line;
            }

            //Checks to see if the @variable is in the symbolDictionary.  If it is, the corresponding value is returned.  If not, it is added and the corresponding value is returned.
            if (symbolDictionary.TryGetValue(line, out string value))
            {
                return '@' + value;
            }
            else
            {
                symbolDictionary.Add(line, variableMemoryLocation.ToString());                
                variableMemoryLocation++;
                return '@' + (variableMemoryLocation - 1).ToString();
            }
        }

    }
}
