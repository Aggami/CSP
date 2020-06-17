using System;
using System.Collections.Generic;
using System.Linq;

namespace CSP_program
{
    public class SudokuCSP : CSP<int>
    {
        private int id;
        private double difficulty;
        private int[][] v;
        public static List<int> D = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private List<Constraint>[] E = new List<Constraint>[81];
        public List<int>[] D2 = new List<int>[81];
        public List<DomainChangeSudoku>[] DC = new List<DomainChangeSudoku>[81];

      
        public double Difficulty { get => difficulty; set => difficulty = value; }
        public int Id { get => id; set => id = value; }

        public SudokuCSP()
        {
        }

        public SudokuCSP(int id, double difficulty, int[][] values)
        {
            this.Id = id;
            this.Difficulty = difficulty;
            this.v = values;
            prepareConstraints();
            for (int i=0; i<81; i++)
            {
                if (v[i / 9][i % 9]>0)
                {
                    D2[i] = new List<int> { v[i / 9][i % 9] };

                } 
                else D2[i] = new List<int>(D);
            }

            for (int i = 0; i<81; i++)
            {
                if (v[i / 9][i % 9] > 0) adjustDomainsForward(i);
            }

            
        }


        public void setV(int currV, int val)
        {
            v[currV / 9][currV % 9] = val;
        }

        public void setStartV(int currV)
        {
            v[currV / 9][currV % 9] = 0;

        }

        public int getStartValue()
        {
            return 0;
        }

        public int getV(int currV)
        {
            return v[currV / 9][currV % 9];
        }

        public List<int> getD(int curV)
        {
            return D2[curV];
        }

        public bool checkConstraints(int currV)
        {

            int x = currV / 9;
            int y = currV % 9;

            //sprawdzenie rzędu
            int value = v[x][y];
            for (int i = 0; i < 9; i++)
            {
                if (i == y) continue;
                if (v[x][i] == value) return false;
            }

            //sprawdzenie kolumny
            for (int i = 0; i < 9; i++)
            {
                if (i == x) continue;
                if (v[i][y] == value) return false;
            }

            //sprawdzenie małych kwadratów
            foreach(Constraint c in E[x * 9 + y])
            {
                if (v[c.X][c.Y] == value) return false;
            }


            return true;

        }

        public void fillFromString(string s)
        {

            int iter = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (s[iter] == '.') v[i][j] = 0;
                    else
                    {
                        v[i][j] = s[iter] - 48;
                    }
                    iter++;
                }
            }
        }

        public string printFromString(string s)
        {
            string sol = "";
            int iter = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sol+=s[iter]+" ";
                    iter++;
                }
                sol += "\n";
            }
            return sol;
        }

        public void printSudoku()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j=0; j<9; j++)
                {
                    if (v[i][j] == 0) Console.Write("-" + " ");
                    else Console.Write(v[i][j] + " ");
                }
                Console.WriteLine();
            }

        }


        public string getSolution()
        {
            string s = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    s += v[i][j];
                }
            }
            return s;
        }

        public int getNumOfV()
        {
            return 81;
        }

        public string Type()
        {
            return "SUDOKU";
        }

        public bool startValuesFlag()
        {
            return true;
        }

        public int getNumOfE(int cv)
        {
            return E[cv].Count;
        }

        public static SudokuCSP[] readSudokus(string filePath)
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);

            SudokuCSP[] sudokus = new SudokuCSP[lines.Length - 1];
            for (int i = 1; i < lines.Length; i++)
            {
                sudokus[i - 1] = createFromString(lines[i]);
            }

            return sudokus;

        }

        public int[] getRows()
        {
            int[] rfilled = new int[9]; 

            for (int i=0; i<9; i++)
            {
                for (int j = 0; j<9; j++)
                {
                    if (D2[i * 9 + j].Count == 1) rfilled[i]++;
                }
            }

            return rfilled;
        }


        public int[] getColumns()
        {
            int[] rfilled = new int[9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (D2[j * 9 + i].Count == 1) rfilled[i]++;
                }
            }

            return rfilled;
        }

        public void printSudokuState()
        {
            Console.WriteLine("Id: " + this.Id);
            Console.WriteLine("Difficulty: " + this.Difficulty);
            for (int i = 0; i < 9; i++)
            {
                string s = "";
                for (int j = 0; j < 9; j++)
                {
                    s += v[i][j];
                }
                Console.WriteLine(s);
            }
        }


        private static SudokuCSP createFromString(string s)
        {
            string[] values = s.Split(';');
            int id = Int32.Parse(values[0].Replace(";", ""));
            double difficulty = Double.Parse(values[1].Replace(";", "").Replace(".", ","));
            int[][] v = new int[9][];

            int iter = 0;

            for (int i = 0; i < 9; i++)
            {
                v[i] = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    if (values[2][iter] == '.') v[i][j] = 0;
                    else
                    {
                        v[i][j] = values[2][iter] - 48;
                    }
                    iter++;
                }
            }

            SudokuCSP sudoku = new SudokuCSP(id, difficulty, v);

            sudoku.prepareConstraints();

            return sudoku;
        }

        //przygotowanie ograniczeń
        private void prepareConstraints()
        {

            Constraint c;
            for (int i = 0; i < 9; i = i + 3)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0)
                    {
                        E[9 * i + j] = new List<Constraint>();
                        c = new Constraint(i + 1, j + 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 2, j + 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 1, j + 2);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 2, j + 2);
                        E[9 * i + j].Add(c);
                    }

                    if (j % 3 == 1)
                    {
                        E[9 * i + j] = new List<Constraint>();
                        c = new Constraint(i + 1, j - 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 2, j - 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 1, j + 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 2, j + 1);
                        E[9 * i + j].Add(c);
                    }

                    if (j % 3 == 2)
                    {
                        E[9 * i + j] = new List<Constraint>();
                        c = new Constraint(i + 1, j - 2);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 2, j - 2);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 1, j - 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 2, j - 1);
                        E[9 * i + j].Add(c);
                    }
                }
            }
            for (int i = 1; i < 9; i = i + 3)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0)
                    {
                        E[9 * i + j] = new List<Constraint>();
                        c = new Constraint(i - 1, j + 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 1, j + 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 1, j + 2);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 1, j + 2);
                        E[9 * i + j].Add(c);
                    }

                    if (j % 3 == 1)
                    {
                        E[9 * i + j] = new List<Constraint>();
                        c = new Constraint(i - 1, j - 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 1, j - 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 1, j + 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 1, j + 1);
                        E[9 * i + j].Add(c);
                    }

                    if (j % 3 == 2)
                    {
                        E[9 * i + j] = new List<Constraint>();
                        c = new Constraint(i - 1, j - 2);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 1, j - 2);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 1, j - 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i + 1, j - 1);
                        E[9 * i + j].Add(c);
                    }
                }
            }

            for (int i = 2; i < 9; i = i + 3)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0)
                    {
                        E[9 * i + j] = new List<Constraint>();
                        c = new Constraint(i - 2, j + 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 1, j + 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 2, j + 2);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 1, j + 2);
                        E[9 * i + j].Add(c);
                    }

                    if (j % 3 == 1)
                    {
                        E[9 * i + j] = new List<Constraint>();
                        c = new Constraint(i - 2, j - 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 1, j - 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 2, j + 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 1, j + 1);
                        E[9 * i + j].Add(c);
                    }

                    if (j % 3 == 2)
                    {
                        E[9 * i + j] = new List<Constraint>();
                        c = new Constraint(i - 2, j - 2);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 1, j - 2);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 2, j - 1);
                        E[9 * i + j].Add(c);
                        c = new Constraint(i - 1, j - 1);
                        E[9 * i + j].Add(c);
                    }
                }
            }


        }

        public void fillBoard()
        {
        }

        public int getVLength(int v)
        {
            return 1;
        }

        public void adjustDomainsForward(int numV, List<int>toCheck)
        {
            DC[numV] = new List<DomainChangeSudoku>();
            int val = v[numV / 9][numV % 9];
            int row = numV / 9;
            int col = numV %9;

            for (int i=0; i<9; i++)
            {
                if (toCheck.Contains(i * 9 + col) &&D2[i*9+col].Contains(val))
                {
                    DC[numV].Add(new DomainChangeSudoku(i * 9 + col, val));
                    D2[i * 9 + col].Remove(val);
                }
            }

            for (int i = 0; i < 9; i++)
            {
                if (toCheck.Contains(row * 9 + i) &&D2[row * 9 + i].Contains(val))
                {
                    DC[numV].Add(new DomainChangeSudoku(row * 9 + i, val));
                    D2[row * 9 + i].Remove(val);
                }
            }

            foreach (Constraint c in E[numV])
            {
                if (toCheck.Contains(c.X * 9 + c.Y)){
                    if (D2[c.X * 9 + c.Y].Contains(val))
                    {
                        DC[numV].Add(new DomainChangeSudoku(c.X * 9 + c.Y, val));
                        D2[c.X * 9 + c.Y].Remove(val);
                    }
                }
            }

        }

        public void adjustDomainsForward(int numV)
        {
            List<int> list = new List<int>();
            for (int i=0; i<81; i++)
            {
                if (v[i / 9][i % 9] == 0) list.Add(i); 
            }
            adjustDomainsForward(numV, list);
        }

        public void bringBackDomains(int numV)
        {
            foreach(DomainChangeSudoku dcs in DC[numV])
            {
                     D2[dcs.V].Add(dcs.Val);           
            }
            DC[numV] = null;
        }



        public bool checkIfAnyEmptyD(List<int> toCheck)
        {
            foreach(int i in toCheck)
            {
                if (D2[i].Count == 0) return true;
            }
            return false;
        }

        public int numOfValues(int val)
        {
            int n = 0;
            for(int i = 0; i<81; i++)
            {
                if (v[i / 9][i % 9] == val) n++; 
            }

            return n;
        }
    }


    public class Constraint{
        int x;
        int y;

        public Constraint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public string toString()
        {
            return x + ", " + y;
        }
    }


    public class DomainChangeSudoku
    {
        int v;
        int val;

        public DomainChangeSudoku(int v, int word)
        {
            this.V = v;
            this.Val = word;
        }

        public int V { get => v; set => v = value; }
        public int Val { get => val; set => val = value; }
    }



}
