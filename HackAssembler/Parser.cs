using System;
using System.IO;
using System.Collections.Generic;

namespace HackAssembler
{
    class Parser
    {
        //List used to store the lines of code during the assembly -> machine code conversion process.
        private List<string> LineList { get; set; }

        //Holder for the path of the .asm file
        private string FilePath { get; set; }


        //Constructor needs a valid .asm file.
        public Parser(string filePath)
        {
            FilePath = filePath;
            LineList = new List<string>();

        }

        //Reads every line of the .asm file.  Removes comments and whitespace, and adds each line to LineList.
        public void ParseFile()
        {
            Symbol symbol = new Symbol();
            string tempString;

            //Keeps track of the line number.
            int lineNumber = 0;

            //First pass.  Goes through every line in the .asm file and...
            foreach (string line in File.ReadLines(FilePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                //...if it has a comment, the comment is removed and/or whitespace is removed...
                if (line.Contains('/'))
                {
                    if (line.IndexOf('/') == 0)
                    {
                        continue;
                    }

                    tempString = line.Remove(line.IndexOf('/'));
                    tempString = tempString.Trim();
                }
                else
                {
                    tempString = line.Trim();
                }

                //...if it is a label (indicated by having a '('), the label is processed and added to symbol dictionary.
                if (tempString[0] == '(')
                {
                    symbol.AddLabel(tempString, lineNumber);
                    continue;
                }

                //The final line value, if it made it through all of the above conditions, is added to the LineList.
                LineList.Add(tempString);
                lineNumber++;
            }

            //Second pass.  Goes through every line in the LineList and changes variables/symbols to their corresponding decimal values.
            for (int i = 0; i < LineList.Count; i++)
            {
                if (LineList[i][0] == '@')
                {
                    LineList[i] = symbol.ProcessVariable(LineList[i]);
                }
            }
        }

        //Reads LineList entries one at a time, determines whether the line is an 'A' or 'C' command, and executes the corresponding binary conversion method.  
        //It then replaces the LineList entry with this binary value.
        public void ParseLines()
        {
            Binary binary = new Binary();
            for (int i = 0; i < LineList.Count; i++)
            {
                //If the line begins with an '@' char, it is an 'A' command.
                if (LineList[i][0] == '@')
                {
                    //Only sends the value after the '@' symbol to the method.
                    LineList[i] = binary.ConvertA(LineList[i].Substring(1));
                }

                //If the line does not begin with an '@' char, it is a 'C' command.
                else
                {
                    LineList[i] = binary.ConvertC(LineList[i]);
                }
            }
        }

        //Displays the content of LineList on the console
        public void DisplayLineList()
        {
            foreach (string line in LineList)
            {
                Console.WriteLine(line);
            }
        }

        //Writes the contents of LineList to a .hack text file.
        public string WriteToFile()
        {
            string outputFilePath = FilePath.Remove(FilePath.Length - 3) + "hack";

            File.WriteAllLines(outputFilePath, LineList);

            return outputFilePath;
        }
    }
}
