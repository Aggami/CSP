using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CSP_program
{
    class Program
    {
        static void Main(string[] args)
        {


            //string puzzlePath = Path.Combine("/Users/aggami/Library/Mobile Documents/com~apple~CloudDocs/Studia/Semestr 6/SIiIW/CSP/CSP/", "Jolka", "puzzle1");
            //string wordsPath = Path.Combine("/Users/aggami/Library/Mobile Documents/com~apple~CloudDocs/Studia/Semestr 6/SIiIW/CSP/CSP/", "Jolka", "words1");

            string sudokuPath = Path.Combine("/Users/aggami/Library/Mobile Documents/com~apple~CloudDocs/Studia/Semestr 6/SIiIW/CSP/CSP/", "Sudoku.csv");

            SudokuCSP[] sudokus = SudokuCSP.readSudokus(sudokuPath);

            SudokuCSP[] sudokuToExperimentWith = new SudokuCSP[] { sudokus[14], sudokus[25], sudokus[35], sudokus[41], sudokus[44] };

            CSPExperiments<int>.compareMethodsByDifficulty(sudokus);
        }

    }
}
