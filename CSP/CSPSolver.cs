using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CSP_program
{
    public class CSPSolver<T>
    {
        int numOfChecks = 0;
        int numOfChecksFirstSol = 0;
        int backsFirstSol;
        int backs;
        long startTime;
        int timeFirstSol;
        int timeInMs;
        bool solveForwardFlag = false;

        List<int> solutionsNrOfChecks = new List<int>();

        CSP<T> problem;
        Stopwatch watch;

        VariableHeuristics<T> VariableHeuristics;
        ValueHeuristics<T> ValueHeuristics;

    
        List<string> solutions = new List<string >();

        public List<string> Solutions { get => solutions; set => solutions = value; }
        public int NumOfChecks { get => numOfChecks; set => numOfChecks = value; }
        public int NumOfChecksFirstSol { get => numOfChecksFirstSol; set => numOfChecksFirstSol = value; }
        public int BacksFirstSol { get => backsFirstSol; set => backsFirstSol = value; }
        public int Backs { get => backs; set => backs = value; }
        public long StartTime { get => startTime; set => startTime = value; }
        public int TimeFirstSol { get => timeFirstSol; set => timeFirstSol = value; }
        public int TimeInMs { get => timeInMs; set => timeInMs = value; }

        public CSPSolver()
        {
            VariableHeuristics = new ByDefinitionHeuristics<T>();
            ValueHeuristics = new ByDefinitionValueHeuristics<T>();
        }

        public CSPSolver(VariableHeuristics<T> variableHeuristics, ValueHeuristics<T> valueHeuristics)
        {
            VariableHeuristics = variableHeuristics;
            ValueHeuristics = valueHeuristics;
        }

        public void solve(CSP<T> problem)
        {
            solveForwardFlag = false;
            solutions.Clear();
            watch = System.Diagnostics.Stopwatch.StartNew();
            this.problem = problem;

            List<int> order;
            order = VariableHeuristics.chooseOrder(problem);

            int currentV = order[0];
            order.Remove(currentV);
            solveRec(problem, currentV, order);
            watch.Stop();
            var elapsedTime = watch.ElapsedMilliseconds;
            TimeInMs = Convert.ToInt32(elapsedTime);
        }

        public void solveForward(CSP<T> problem)
        {

            solveForwardFlag = true;
            solutions.Clear();

            watch = System.Diagnostics.Stopwatch.StartNew();
            this.problem = problem;

            List<int> order;
            order = VariableHeuristics.chooseOrder(problem);

            int currentV = order[0];
            solveRecForward(problem, currentV, order);
            watch.Stop();
            var elapsedTime = watch.ElapsedMilliseconds;
            TimeInMs = Convert.ToInt32(elapsedTime);
        }

        public void solveRec(CSP<T> problem, int currentV, List<int> toCheck)
        {
            int currIndex = toCheck.IndexOf(currentV);
            toCheck.Remove(currentV);
            List<T> domain = new List<T>(problem.getD(currentV));
            domain = ValueHeuristics.chooseOrder(domain, problem);

            foreach (T val in domain)
            {
                NumOfChecks++;
                problem.setV(currentV, val);
                if (problem.checkConstraints(currentV))
                {
                    if (toCheck.Count == 0)
                    {
                        if (solutions.Count == 0)
                        {
                            NumOfChecksFirstSol = NumOfChecks;
                            BacksFirstSol = Backs;
                            TimeFirstSol = Convert.ToInt32(watch.ElapsedMilliseconds);

                        }
                        Solutions.Add(problem.getSolution());
                        solutionsNrOfChecks.Add(NumOfChecks);
                        //problem.fillBoard();
                        //Console.WriteLine("Sukces!");

                        problem.setStartV(currentV);
                        //toCheck.Add(currentV);
                        break;
                    }
                    int next = VariableHeuristics.dynamicallyChooseNext(problem, toCheck, false);
                    solveRec(problem, next, toCheck);
                }

            }
            Backs++;
            problem.setV(currentV, problem.getStartValue());
            toCheck.Insert(0, currentV);
        }

        public void solveRecForward(CSP<T> problem, int currentV, List<int> toCheck)
        {
            int currIndex = toCheck.IndexOf(currentV);
            toCheck.Remove(currentV);
            List<T> domain = new List<T>(problem.getD(currentV));
            domain = ValueHeuristics.chooseOrder(domain, problem);
            foreach (T val in domain)
            {
                //Console.WriteLine(currentV);
                NumOfChecks++;
                problem.setV(currentV, val);
                problem.adjustDomainsForward(currentV, toCheck);
                //zmiana dziedzin
                if (!problem.checkIfAnyEmptyD(toCheck))
                {
                    if (toCheck.Count == 0)
                    {
                        if (solutions.Count == 0)
                        {
                            NumOfChecksFirstSol = NumOfChecks;
                            BacksFirstSol = Backs;
                            TimeFirstSol = Convert.ToInt32(watch.ElapsedMilliseconds);

                        }
                        if (problem.checkConstraints(currentV)) Solutions.Add(problem.getSolution());
                        solutionsNrOfChecks.Add(NumOfChecks);

                        //problem.fillBoard();
                        //Console.WriteLine("Sukces!");


                        problem.bringBackDomains(currentV);
                        problem.setStartV(currentV);
                        //toCheck.Add(currentV);
                        break;
                    }
                    int next = VariableHeuristics.dynamicallyChooseNext(problem, toCheck, true);
                    solveRecForward(problem, next, toCheck);
                }
                problem.bringBackDomains(currentV);

            }
            Backs++;
            problem.setV(currentV, problem.getStartValue());
            //Console.WriteLine(numOfChecks + " " + toCheck.Count +" "+currIndex+" "+currentV);
            if (currIndex < toCheck.Count) toCheck.Insert(currIndex, currentV);
            else toCheck.Add(currentV);
        }



        public int chooseNextToCheck(List<int> toCheck)
        {
                return toCheck[0];
        }


        public string getInfo()
        {
            string s = "";
            s += "Liczba odwiedzonych wezlow do 1. rozw: " + NumOfChecksFirstSol + "\n";
            s += "Czas do znalezienia 1. rozwiazania: " + TimeFirstSol + "\n";
            s += "Liczba nawrotow do 1. rozwiazania: " + BacksFirstSol + "\n";
            s += "Liczba odwiedzonych wezlow: " + NumOfChecks + "\n";
            s += "Czas: " + TimeInMs + "\n";
            s += "Liczba nawrotow: " + Backs + "\n";

            s += "Liczba rozw: " + solutions.Count + "\n";

            foreach (string sol in solutions)
            {

                s += sol + "\n";
            }

            foreach (string sol in solutions)
            {

                s += problem.printFromString(sol) + "\n";
            }



            return s;
        }

        public string getInfoBrief()
        {
            string s = "";
            if (solveForwardFlag) s += "Metoda forward checking \n"; else s += "Metoda backtracking: \n";
            s += "Heurystyki: " + VariableHeuristics.ToString() + " " + ValueHeuristics.ToString()+"\n";
            s += "Wezly do 1. rozw: " + NumOfChecksFirstSol + "   ";
            s += "Czas do  1. rozw.: " + TimeFirstSol + "     ";
            s += "L. nawrotow do 1. rozw.: " + BacksFirstSol + "\n";
            s += "Wezly: " + NumOfChecks + "    ";
            s += "Czas: " + TimeInMs + "    ";
            s += "Liczba nawrotow: " + Backs + "\n";

            s += "Liczba rozw: " + solutions.Count + "\n";

            return s; 
        }

        public string getInfoCSV()
        {
            string s = "";
            if (solveForwardFlag) s += "FC; "; else s += "BT; ";
            s+=VariableHeuristics.ToString()+"; " + ValueHeuristics.ToString()+"; ";
            s+= NumOfChecksFirstSol + "; " + BacksFirstSol + "; " + TimeFirstSol + "; ";
            s+= NumOfChecks + "; "+Backs+"; "+TimeInMs+"; ";
            s += solutions.Count + "; ";
            return s;
        }

        


    }

}
