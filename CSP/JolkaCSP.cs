using System;
using System.Collections.Generic;

namespace CSP_program
{
    public class JolkaCSP : CSP<string>
    {
        int id = 0;
        string[] words;
        char[,] board;
        string[] v;
        int[] wordsLen;

        int wordsNum;
        List<string> availableWords;
        List<string>[] d;
        List<JolkaEdge>[] e;
        Coordinates[] coord;
        int sizeX;
        int sizeY;
        int numOfHorizontal;

        private bool ifWordAvailable = true;

        public List<string>[] D { get => d; set => d = value; }
        public List<JolkaEdge>[] E { get => E1; set => E1 = value; }
        public string[] V { get => v; set => v = value; }
        public List<string> AvailableWords { get => availableWords; set => availableWords = value; }
        public List<JolkaEdge>[] E1 { get => e; set => e = value; }
        public int Id { get => id; set => id = value; }
        public double Difficulty { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<DomainChange>[] DC;

        public JolkaCSP(string[] words, char[,] board, string[] v, int[] wordsLen, List<string>[] d, int id, List<JolkaEdge>[] e, Coordinates[] c, int numOfHorizontal)
        {
            this.words = words;
            this.board = board;
            this.V = v;
            this.wordsLen = wordsLen;
            this.AvailableWords = new List<string>(words);
            this.D = d;
            this.Id = id;
            this.E = e;
            this.coord = c;
            this.numOfHorizontal = numOfHorizontal;
            this.DC = new List<DomainChange>[words.Length];
            for (int i=0; i<DC.Length; i++)
            {
                DC[i] = new List<DomainChange>();
            }

        }

        public JolkaCSP(string[] words, char[,] board, int sizeX, int sizeY, int wordsNum)
        {
            this.wordsNum = wordsNum;
            this.words = words;
            this.board = board;
            this.AvailableWords = new List<string>(words);
            this.D = new List<string>[wordsNum];
            this.E = new List<JolkaEdge>[wordsNum];
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.DC = new List<DomainChange>[words.Length];
        }

        public void adjustDomainsForward(int numV, List<int> toCheck)
        {

            DC[numV] = new List<DomainChange>();
            string word = v[numV];
            foreach (int tc in toCheck)
            {
                if (D[tc].Contains(word))
                {
                    D[tc].Remove(word);
                    DC[numV].Add(new DomainChange(tc, word));
                }
            }

            foreach(JolkaEdge je in E[numV])
            {
               
                int otherV = je.W1 == numV ? je.W2 : je.W2;
                if (!toCheck.Contains(otherV)) continue;
                int otherL = je.W1 == numV ? je.L2 : je.L1;
                int thisL = je.W1 == numV ? je.L1 : je.L2;
                foreach (string w in new List<string>(D[otherV]))
                {
                    if (w[otherL] != word[thisL])
                    {
                        D[otherV].Remove(w);
                        DC[numV].Add(new DomainChange(otherV, w));
                    }
                }
            }

        }


        public void bringBackDomains(int numV)
        {
            foreach(DomainChange change in DC[numV])
            {
                D[change.V].Add(change.Word); 
            }

            DC[numV] = null;
        }


        public bool checkConstraints(int vNum)
        {
            for (int i = 0; i < words.Length; i++)
            {
                if (vNum == i) continue;
                if (v[i] == v[vNum]) return false;
            }
            foreach (JolkaEdge je in E1[vNum])
            {
                if (v[je.W1] != null && v[je.W2] != null)
                {
                    if (v[je.W1][je.L1] != v[je.W2][je.L2]) return false;
                }
            }
            return true;
        }

        public void printJolka()
        {
            string s = "";
            foreach (string word in words)
            {
                s += word + ", ";
            }
            Console.WriteLine(s);
            for (int i = 0; i < words.Length; i++)
            {
                Console.WriteLine("Slowo " + i);
                Console.WriteLine(wordsLen[i]);
                foreach (string w in D[i])
                {
                    Console.Write(w);
                }
                Console.WriteLine();
                foreach (JolkaEdge j in E[i])
                {
                    Console.Write(" " + j);
                }
                Console.WriteLine();
            }

            return;
        }

        public static JolkaCSP readFromFiles(string puzzlePath, string wordsPath)
        {
            string[] puzzles = System.IO.File.ReadAllLines(puzzlePath);
            string[] words = System.IO.File.ReadAllLines(wordsPath);

            JolkaCSP jolka = createFromStrings(words, puzzles, 0);
            return jolka;
        }

        public void printState()
        {
            foreach (string vv in v)
            {
                Console.Write(vv + " ");
            }
            Console.WriteLine();
        }

        public void setV(int currentV, string value)
        {

            v[currentV] = value;
        }

        public void setStartV(int currentV)
        {
            //if (v[currentV] != null) AvailableWords.Add(v[currentV]);
            v[currentV] = null;

        }

        public string getV(int currentV)
        {
            return v[currentV];
        }

        public string getSolution()
        {
            string s = "";
            for (int i = 0; i < words.Length; i++)
            {
                s += " " + i + ". " + v[i];
            }
            return s;
        }

        public string getStartValue()
        {
            return null;
        }

        public List<string> getD(int cur)
        {
            return D[cur];
        }

        public int getNumOfV()
        {
            return words.Length;
        }

        public int getNumOfE(int cv)
        {
            return E[cv].Count;
        }

        public string Type()
        {
            return "JOLKA";
        }

        public bool startValuesFlag()
        {
            return false;
        }

        public void fillBoard(string[] v)
        {
            for(int i= 0; i<numOfHorizontal; i++)
            {
                string word = v[i];
                for(int a=0; a<word.Length; a++)
                {
                    board[coord[i].X, coord[i].Y+a] = word[a];
                }
            }

            for (int i = numOfHorizontal; i < words.Length; i++)
            {
                string word = v[i];
                for (int a = 0; a < word.Length; a++)
                {
                    board[coord[i].X+a, coord[i].Y] = word[a];
                }
            }
        }

        public string printBoard()
        {
            string s = "\n";

            for (int i=0; i<board.GetLength(0); i++)
            {
                for (int j=0; j<board.GetLength(1); j++)
                {
                    s+=board[i, j] + " ";

                }
                s += "\n";

            }
            return s;
        }

        public string printFromString(string s)
        {
            string[] vs = s.Split();
            string[] tempv = new string[words.Length];

            
            for (int i=0; i<words.Length; i++)
            {
                tempv[i] = vs[2 * i + 2].Trim();
            }

            fillBoard(tempv);

            string sol = printBoard();
            return sol;
        }


        private static JolkaCSP createFromStrings(string[] words, string[] puzzle, int id)
        {
            char[,] board = new char[puzzle.Length, puzzle[0].Length];

            int lenw = 0;

            int[] wordLengths = new int[words.Length];
            bool wordStarted = false;
            int numOfHorizontal = 0;
            int wordCounter = 0;


            string[] v = new string[words.Length];
            List<JolkaEdge>[] edges = new List<JolkaEdge>[words.Length];
            List<string>[] d = new List<string>[words.Length];
            Coordinates[] c = new Coordinates[words.Length];

            string[,] boardHelper = new string[puzzle.Length, puzzle[0].Length];


            for (int i = 0; i < puzzle.Length; i++)
            {
                for (int j = 0; j < puzzle[0].Length; j++)
                {
                    board[i, j] = puzzle[i][j];
                }
            }


            for (int i = 0; i < puzzle.Length; i++)
            {
                for (int j = 0; j < puzzle[0].Length; j++)
                {
                    //początek nowego słowa
                    if ((!wordStarted) && (board[i, j] == '_'))
                    {
                        if (j != puzzle[0].Length - 1)
                        {
                            if (board[i, j + 1] == '_')
                            {
                                wordStarted = true;
                                boardHelper[i, j] = wordCounter + " " + (lenw);
                                lenw++;
                                c[wordCounter] = new Coordinates(i, j);
                            }
                        }
                        continue;

                    }
                    //koniec słowa
                    if (((wordStarted) && (board[i, j] == '#')) || ((wordStarted) && (j == puzzle[0].Length - 1)))
                    {
                        if (board[i, j] == '_')
                        {
                            boardHelper[i, j] = wordCounter + " " + (lenw);
                            lenw++;
                        }
                        wordStarted = false;
                        if (lenw > 1)
                        {
                            v[wordCounter] = null;
                            edges[wordCounter] = new List<JolkaEdge>();
                            wordLengths[wordCounter] = lenw;
                            wordCounter++;
                            numOfHorizontal++;
                        }
                        lenw = 0;

                        continue;
                    }

                    //środek słowa
                    if ((wordStarted) && (board[i, j] == '_'))
                    {
                        boardHelper[i, j] = wordCounter + " " + (lenw);
                        lenw++;
                    }



                }



            }

            List<JolkaEdge> jes = new List<JolkaEdge>();

            for (int i = 0; i < puzzle[0].Length; i++)
            {
                for (int j = 0; j < puzzle.Length; j++)
                {
                    if ((!wordStarted) && (board[j, i] == '_'))
                    {
                        if (j != puzzle.Length - 1)
                        {
                            if (board[j + 1, i] == '_')
                            {
                                wordStarted = true;
                                if (boardHelper[j, i] != null)
                                {
                                    string[] bh = boardHelper[j, i].Split();
                                    int wordNum = Int32.Parse(bh[0]);
                                    int letNum = Int32.Parse(bh[1]);
                                    JolkaEdge je = new JolkaEdge(wordNum, letNum, wordCounter, lenw);
                                    jes.Add(je);
                                    edges[wordNum].Add(je);
                                }
                                c[wordCounter] = new Coordinates(j, i);
                                lenw++;
                            }
                        }
                        continue;
                    }
                    //koniec słowa
                    if (((wordStarted) && (board[j, i] == '#')) || ((wordStarted) && (j == puzzle.Length - 1)))
                    {

                        if (j == puzzle.Length - 1)
                        {
                            if (boardHelper[j, i] != null)
                            {
                                string[] bh = boardHelper[j, i].Split();
                                int wordNum = Int32.Parse(bh[0]);
                                int letNum = Int32.Parse(bh[1]);
                                JolkaEdge je = new JolkaEdge(wordNum, letNum, wordCounter, lenw);
                                jes.Add(je);
                                edges[wordNum].Add(je);

                            }
                        }

                        if (board[j, i] == '_') lenw++;
                        wordStarted = false;
                        if (lenw > 1)
                        {
                            v[wordCounter] = null;
                            wordLengths[wordCounter] = lenw;
                            edges[wordCounter] = jes;
                            wordCounter++;
                        }
                        lenw = 0;
                        jes = new List<JolkaEdge>();

                        continue;

                    }


                    if ((wordStarted) && (board[j, i] == '_'))
                    {
                        if (boardHelper[j, i] != null)
                        {
                            string[] bh = boardHelper[j, i].Split();
                            int wordNum = Int32.Parse(bh[0]);
                            int letNum = Int32.Parse(bh[1]);
                            JolkaEdge je = new JolkaEdge(wordNum, letNum, wordCounter, lenw);
                            jes.Add(je);
                            edges[wordNum].Add(je);
                        }
                        lenw++;

                    }
                }

            }



            JolkaCSP jolka = new JolkaCSP(words, board, v, wordLengths, new List<string>[words.Length], id, edges, c, numOfHorizontal);
            jolka.findD();
            return jolka;

        }

        private void findD()
        {
            if (wordsLen == null) return;
            D = new List<string>[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                D[i] = new List<string>();
                foreach (string word in words)
                {
                    if (wordsLen[i] == word.Length) D[i].Add(word);
                }
            }
        }

        public int getVLength(int v)
        {
            return wordsLen[v];
        }

        public bool checkIfAnyEmptyD(List<int> toCheck)
        {
            foreach (int tc in toCheck)
            {
                if (D[tc].Count == 0) return true;
            }
            return false;
        }

        public int numOfValues(string val)
        {
            throw new NotImplementedException();
        }

        public int[] getRows()
        {
            throw new NotImplementedException();
        }

        public int[] getColumns()
        {
            throw new NotImplementedException();
        }
    }

    public class JolkaEdge
    {
        int w1;
        int l1;

        int w2;
        int l2;

        public JolkaEdge(int w1, int l1, int w2, int l2)
        {
            this.W1 = w1;
            this.L1 = l1;
            this.W2 = w2;
            this.L2 = l2;
        }

        public int W1 { get => w1; set => w1 = value; }
        public int L1 { get => l1; set => l1 = value; }
        public int W2 { get => w2; set => w2 = value; }
        public int L2 { get => l2; set => l2 = value; }

        public override string ToString()
        {
            return "(" + W1 + "," + L1 + ")" + "(" + W2 + "," + L2 + ")";
        }
    }

    public class Coordinates{
        int x;
        int y;

        public Coordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
    }

    public class DomainChange
    {
        int v;
        string word;

        public DomainChange(int v, string word)
        {
            this.V = v;
            this.word = word;
        }

        public string Word { get => word; set => word = value; }
        public int V { get => v; set => v = value; }
    }

   
}