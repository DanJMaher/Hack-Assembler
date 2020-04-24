using System;
using System.Collections.Generic;
using System.Text;

namespace HackAssembler
{
    class Binary
    {
        //CArray holds the three separate parts of a c-instruction during c-instruction processing.
        public string[] CArray { get; private set; }

        //These arrays hold the binary conversion values for the three types of c-instructions.  Jump, Destination, and Compute.
        private static string[,] _JumpArray = new string[7, 2]
        {
            {"JGT","001" },
            {"JEQ","010" },
            {"JGE","011" },
            {"JLT","100" },
            {"JNE","101" },
            {"JLE","110" },
            {"JMP","111" }
        };
        private static string[,] _DestArray = new string[7, 2]
        {
            {"M","001" },
            {"D","010" },
            {"MD","011" },
            {"A","100" },
            {"AM","101" },
            {"AD","110" },
            {"AMD","111" }
        };
        private static string[,] _CompArray = new string[18, 2]
        {
            {"0","101010" },
            {"1","111111" },
            {"-1","111010" },
            {"D","001100" },
            {"A","110000" },
            {"!D","001101" },
            {"!A","110001" },
            {"-D","001111" },
            {"-A","110011" },
            {"D+1","011111" },
            {"A+1","110111"},
            {"D-1","001110" },
            {"A-1","110010" },
            {"D+A","000010" },
            {"D-A","010011" },
            {"A-D","000111" },
            {"D&A","000000" },
            {"D|A","010101" }
        };

        public string ConvertA(string aCommand)
        {
            //Converts the input string to a binary number.
            string aBinary = Convert.ToString(Convert.ToInt32(aCommand), 2);

            //Adds leading zeros to the binary number until the length equals 16 (for the 16-bit command).  Returns the final value.
            int missingDigits = 16 - aBinary.Length;
            while (missingDigits > 0)
            {
                aBinary = "0" + aBinary;
                missingDigits--;
            }
            return aBinary;
        }

        public string ConvertC(string cCommand)
        {
            //Initializes the CArray to empty.  This array is used to hold the different parts of the C instruction.  [0] = comp, [1] = dest, [2] = jump.
            CArray = new string[3] { "", "000", "000" };
            //Initializes the aBit to '0'
            string aBit = "0";

            //Determines whether the line has a jump instruction.
            if (cCommand.Contains(';'))
            {
                //Creates a substring beginning at the character after the ';' delimiter.
                CArray[2] = cCommand.Substring(cCommand.IndexOf(';') + 1);

                for (int i = 0; i < _JumpArray.GetLength(0); i++)
                {
                    if (CArray[2] == _JumpArray[i, 0])
                    {
                        CArray[2] = _JumpArray[i, 1];
                        break;
                    }
                }

                cCommand = cCommand.Remove(cCommand.IndexOf(';'));
            }

            //Determines whether the line has a destination instruction.
            if (cCommand.Contains('='))
            {
                //Creates a substring from position [0] to the '=' deliminator
                CArray[1] = cCommand.Substring(0, (cCommand.IndexOf('=')));

                for (int i = 0; i < _DestArray.GetLength(0); i++)
                {
                    if (CArray[1] == _DestArray[i, 0])
                    {
                        CArray[1] = _DestArray[i, 1];
                        break;
                    }
                }

                cCommand = cCommand.Remove(0, (cCommand.IndexOf('=')+1));
            }

            //The _CompArray uses A for both A and M.  This statement changes M's to A's and changes the aBit to "1".  FYI at this point, the only value left in cCommand is the comp instruction.
            if (cCommand.Contains('M'))
            {
                cCommand = cCommand.Replace('M', 'A');
                aBit = "1";
            }
            for (int i = 0; i < _CompArray.GetLength(0); i++)
            {
                if (cCommand == _CompArray[i,0])
                {
                    CArray[0] = _CompArray[i, 1];
                    break;
                }
            }

            return "111" + aBit + CArray[0] + CArray[1] + CArray[2];
        }




    }
}
