using System;
using System.Collections.Generic;

namespace CSP_program
{
    public interface CSP<T>
    {

        public void setV(int currentV, T value);
        public void setStartV(int currentV);

        public T getV(int currentV);
        public bool checkConstraints(int cu);
        public string getSolution();
        public T getStartValue();

        public int getNumOfV();
        public int getNumOfE(int cv);
        public int getVLength(int v);

        

        public void adjustDomainsForward(int numV, List<int> toCheck);
        public void bringBackDomains(int numV);

        public bool startValuesFlag();

        public string Type();

        public List<T> getD(int cur);

        public int[] getRows();
        public int[] getColumns();

        public bool checkIfAnyEmptyD(List<int> toCheck);

        public string printFromString(string s);

        public int numOfValues(T val);

        int Id {
            get; set;
        }

        double Difficulty
        {
            get; set;
        }

    }
}
