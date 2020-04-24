using System;
using System.IO;

namespace HackAssembler
{
    class Program
    {
        static void Main(string[] args)
        {
            //Main method handles all console commands, file retrieval, and method calls.

            //The parser object does all the heavy lifting, see method descriptions below.
            Parser parser;
            string inputFilePath;

            while (true)
            {
                Console.Write("Enter a valid .asm file path: ");
                inputFilePath = Console.ReadLine();

                if (!File.Exists(inputFilePath))
                {
                    Console.WriteLine("\n{0} is not a valid file path!", inputFilePath);
                    continue; ;
                }
                if (!(Path.GetExtension(inputFilePath) == ".asm"))
                {
                    Console.WriteLine("File needs to end in .asm!", inputFilePath);
                    continue;
                }
                break;
            }
            parser = new Parser(inputFilePath);

            //Imports the file and executes two passes.  The first pass adds all label to the Symbol Dictionary, and the second pass both adds variables to the Symbol Dictionary and retrives symbol values.
            parser.ParseFile();

            //Just console flavor for displaying the processed .asm file.
            parser.DisplayLineList();

            //Converts line-by-line to 16-bit binary values.
            parser.ParseLines();

            //Console flavor for displaying binary values.
            parser.DisplayLineList();

            //Calls the method that writes the .hack file in the same directory as the input .asm file.
            Console.WriteLine("\nExport complete!\n\n File was saved at location {0}", parser.WriteToFile());
            Console.Write("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
