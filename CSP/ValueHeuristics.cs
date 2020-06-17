using System;
using System.Collections.Generic;

namespace CSP_program
{
    public interface ValueHeuristics<T>
    {
        public List<T> chooseOrder(List<T> list, CSP<T> problem);
       
    }


    public class ByDefinitionValueHeuristics<T> : ValueHeuristics<T>
    {
        public List<T> chooseOrder(List<T> list, CSP<T> problem)
        {
            list.Sort();
            return list;
        }

        public override string ToString()
        {
            return "VAL-BY DEFINITION";
        }
    }


    public class RandomValueHeuristics<T> : ValueHeuristics<T>
    {
        public List<T> chooseOrder(List<T> list, CSP<T> problem)
        {
            List<T> listCopy = new List<T>(list);
            List<T> secondList = new List<T>();
            Random r = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                int x = r.Next(listCopy.Count);
                secondList.Add(listCopy[x]);
                listCopy.RemoveAt(x);

            }

            return secondList;
        }

        public override string ToString()
        {
            return "VAL-RANDOM";
        }
    }

    public class NumOfExistingValsValueHeuristics<T> : ValueHeuristics<T>
    {
        public List<T> chooseOrder(List<T> list, CSP<T> problem)
        {
            List<T> listCopy = new List<T>(list);
            List<T> order = new List<T>();

            for (int i = 0; i < list.Count; i++)
            {
                int maxE = -1;
                T num=default(T);
                foreach (T tochoose in listCopy)
                {
                    if (problem.numOfValues(tochoose) > maxE)
                    {
                        maxE = problem.numOfValues(tochoose);
                        num = tochoose;
                    }
                }
                order.Add(num);
                listCopy.Remove(num);
                //Console.Write(num);
            }
            return order;
        }

        public override string ToString()
        {
            return "VAL - EXISTING VALS";
        }
    }
}
