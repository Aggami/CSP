using System;
using System.Collections.Generic;

namespace CSP_program
{
    public interface VariableHeuristics<T>
    {
        public List<int> chooseOrder(CSP<T> problem);
        public int dynamicallyChooseNext(CSP<T> problem, List<int> toCheck, bool isDynamic);
    }


    public class ByDefinitionHeuristics<T> : VariableHeuristics<T>
    {
        public List<int> chooseOrder(CSP<T> problem)
        {
            List<int> toChooseFrom = new List<int>();


            if (problem.startValuesFlag())
            {
                for (int i = 0; i < problem.getNumOfV(); i++)
                {
                    if (problem.getV(i).Equals(problem.getStartValue())) toChooseFrom.Add(i);
                }
            }
            else
            {
                for (int i = 0; i < problem.getNumOfV(); i++)
                {
                    toChooseFrom.Add(i);
                }
            }

            return toChooseFrom;
        }

        virtual public int dynamicallyChooseNext(CSP<T> problem, List<int> toCheck, bool isDynamic)
        {
            return toCheck[0];
        }

        public override string ToString()
        {
            return "VAR-BY DEFINITION";
        }
    }

    public class RandomVariableHeuristics<T> : VariableHeuristics<T>
    {
        public List<int> chooseOrder(CSP<T> problem)
        {
            List<int> toChooseFrom = new List<int>();

            if (problem.startValuesFlag())
            {
                for (int i = 0; i < problem.getNumOfV(); i++)
                {
                    if (problem.getV(i).Equals(problem.getStartValue())) toChooseFrom.Add(i);
                }
            }
            else
            {
                for (int i = 0; i < problem.getNumOfV(); i++)
                {
                    toChooseFrom.Add(i);
                }
            }

            int listSize = toChooseFrom.Count;
            List<int> secondList = new List<int>();
            Random r = new Random();
            for (int i = 0; i < listSize; i++)
            {
                int x = r.Next(toChooseFrom.Count);
                secondList.Add(toChooseFrom[x]);
                toChooseFrom.RemoveAt(x);
            }

            return secondList;

        }

        public int dynamicallyChooseNext(CSP<T> problem, List<int> toCheck, bool isDynamic)
        {
            return toCheck[0];
        }
    }

    public class ByNumOfConstraintsHeuristics<T> : VariableHeuristics<T>
    {
        public List<int> chooseOrder(CSP<T> problem)
        {
            List<int> toChooseFrom = new List<int>();
            List<int> order = new List<int>();


            for (int i = 0; i < problem.getNumOfV(); i++)
            {
                toChooseFrom.Add(i);
            }

            for (int i = 0; i < problem.getNumOfV(); i++)
            {
                int maxE = -1;
                int num = 0;
                foreach (int tochoose in toChooseFrom)
                {
                    if (problem.getNumOfE(tochoose) > maxE)
                    {
                        maxE = problem.getNumOfE(tochoose);
                        num = tochoose;
                    }
                }
                order.Add(num);
                toChooseFrom.Remove(num);
                //Console.Write(num);
            }
            return order;
        }

        public int dynamicallyChooseNext(CSP<T> problem, List<int> toCheck, bool isDynamic)
        {
            int maxE = -1;
            int num = 0;
            for (int i = 0; i < toCheck.Count; i++)
            {
                if (problem.getNumOfE(toCheck[i]) > maxE)
                {
                    maxE = problem.getNumOfE(toCheck[i]);
                    num = toCheck[i];
                }
            }
            return num;
        }

        public override string ToString()
        {
            return "VAR-BY CONSTRAINTS DYNAMIC";
        }
    }


    public class ByWordLengthHeuristics<T> : VariableHeuristics<T>
    {
        public List<int> chooseOrder(CSP<T> problem)
        {
            List<int> toChooseFrom = new List<int>();
            List<int> order = new List<int>();


            for (int i = 0; i < problem.getNumOfV(); i++)
            {
                toChooseFrom.Add(i);
            }

            for (int i = 0; i < problem.getNumOfV(); i++)
            {
                int maxE = -1;
                int num = 0;
                foreach (int tochoose in toChooseFrom)
                {
                    if (problem.getVLength(tochoose) > maxE)
                    {
                        maxE = problem.getNumOfE(tochoose);
                        num = tochoose;
                    }
                }
                order.Add(num);
                toChooseFrom.Remove(num);
                //Console.Write(num);
            }
            return order;
        }

        public int dynamicallyChooseNext(CSP<T> problem, List<int> toCheck, bool isDynamic)
        {
            return toCheck[0];
        }

        public override string ToString()
        {
            return "VAR-BY WORDLENGTH";
        }
    }

    public class ByDomainVarHeuristics<T> : VariableHeuristics<T>
    {
        public List<int> chooseOrder(CSP<T> problem)
        {
            List<int> toChooseFrom = new List<int>();
            List<int> order = new List<int>();


            for (int i = 0; i < problem.getNumOfV(); i++)
            {
                if (problem.getD(i).Count > 1) toChooseFrom.Add(i);
            }

            int len = toChooseFrom.Count;
            for (int i = 0; i < len; i++)
            {
                int minE = Int32.MaxValue;
                int num = 0;
                foreach (int tochoose in toChooseFrom)
                {
                    if (problem.getD(tochoose).Count < minE)
                    {
                        minE = problem.getD(tochoose).Count;
                        num = tochoose;
                    }

                    
                    //Console.Write(num);
                }
                order.Add(num);
                toChooseFrom.Remove(num);
            }

             

                return order;
            
        }

        public int dynamicallyChooseNext(CSP<T> problem, List<int> toCheck, bool isDynamic)
        {
            if (!isDynamic) return toCheck[0];

            int minE = Int32.MaxValue;
            int num = 0;
            for (int i = 0; i < toCheck.Count; i++)
            {
                if (problem.getD(toCheck[i]).Count < minE)
                {
                    minE = problem.getD(toCheck[i]).Count;
                    num = toCheck[i];
                }
            }
            return num;
        }

        public override string ToString()
        {
            return "VAR-BY DOMAIN DYNAMIC";
        }

       
    }
     

    public class ByBestRowsStaticHeuristic<T> : VariableHeuristics<T>
    {
        public List<int> chooseOrder(CSP<T> problem)
        {
            int[] rows = problem.getRows();

            List<int> rowOrder = new List<int>();


            for (int i = 0; i < 9; i++)
            {
                int maxE = -1;
                int num = -1;
                for (int j = 0; j < 9; j++)
                {
                    if (rows[j] > maxE)
                    {
                        num = j;
                        maxE = rows[j];
                    }
                }
                rowOrder.Add(num);
                rows[num] = Int32.MinValue;

            }

           

            List<int> varOrder = new List<int>();

            foreach (int row in rowOrder)
            {
                for (int j = 0; j < 9; j++)
                {
                    varOrder.Add(row * 9 + j);
                }
            }
            return varOrder;


        }

        public int dynamicallyChooseNext(CSP<T> problem, List<int> toCheck, bool isDynamic)
        {
            return toCheck[0];
        }

        public override string ToString()
        {
            return "VAR-BY ROWS";
        }

    }

        public class ByBestRowsAndColumnsStaticHeuristic<T> : VariableHeuristics<T>
    {
        public List<int> chooseOrder(CSP<T> problem)
        {
            int[] rows = problem.getRows();
            int[] columns = problem.getColumns();

            List<int> rowOrder = new List<int>();
            List<int> columnOrder = new List<int>();


            for(int i= 0; i<9; i++)
            {
                int maxE = -1;
                int num = -1;
                for (int j=0; j<9; j++)
                {
                    if (rows[j] > maxE)
                    {
                        num = j;
                        maxE = rows[j];
                    }
                }
                rowOrder.Add(num);
                rows[num] = Int32.MinValue;

            }

            for (int i = 0; i < 9; i++)
            {
                int maxE = -1;
                int num = -1;
                for (int j = 0; j < 9; j++)
                {
                    if (columns[j] > maxE)
                    {
                        num = j;
                        maxE = columns[j];
                    }
                }
                columnOrder.Add(num);
                columns[num] = Int32.MinValue;

            }

            List<int> varOrder = new List<int>();

            foreach (int row in rowOrder)
            {
                for(int j =0; j<9; j++)
                {
                    varOrder.Add(row * 9 + columnOrder[j]);
                }
            }
            return varOrder;


        }

        public int dynamicallyChooseNext(CSP<T> problem, List<int> toCheck, bool isDynamic)
        {
            return toCheck[0];
        }

        public override string ToString()
        {
            return "VAR-BY ROWS AND COLUMNS";
        }
    }



}
