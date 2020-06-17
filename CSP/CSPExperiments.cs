using System;
namespace CSP_program
{
    public class CSPExperiments<T>
    {
        public const string FOLDERPATH = @"/Users/aggami/Library/Mobile Documents/com~apple~CloudDocs/Studia/Semestr 6/SIiIW/";

        public CSPExperiments()
        {
        }

        public static void compareMethodsExperiment(CSP<T> problem,VariableHeuristics<T> varHeu=null,ValueHeuristics<T> valHeu=null)
        {
            if (varHeu == null) varHeu = new ByDefinitionHeuristics<T>();
            if (valHeu == null) valHeu = new ByDefinitionValueHeuristics<T>();

            var file = new System.IO.StreamWriter(FOLDERPATH+"porownanieAlgorytmow"+problem.GetType()+problem.Id+varHeu.ToString()+valHeu.ToString()+".txt");
            file.AutoFlush = true;
            //to do:
            file.WriteLine("Backtracking");

            string startLine = "NrWyk; Alg; VarHeu; ValHeu; Nods1Sol; Backs1Sol; Time1Sol; Nods; Backs; Time; L.rozw;";
            file.WriteLine(startLine);

            for (int i=0; i<10; i++)
            {
                CSPSolver<T> algBacktracking = new CSPSolver<T>(varHeu, valHeu);
                algBacktracking.solve(problem);
                file.WriteLine(i+"; "+algBacktracking.getInfoCSV());
                
            }

            file.WriteLine("Forward checking");
            file.WriteLine(startLine);

            for (int i = 0; i < 10; i++)
            {
                CSPSolver<T> algForwardchecking = new CSPSolver<T>(varHeu, valHeu);
                algForwardchecking.solveForward(problem);
                file.WriteLine(i + "; "+algForwardchecking.getInfoCSV());
            }

            file.Close();

        }
        
        public static void compareMethodsExperimentForArray(CSP<T>[] problems, VariableHeuristics<T> varHeu = null, ValueHeuristics<T> valHeu = null)
        {
            foreach (CSP<T> problem in problems)
            {
                compareMethodsExperiment(problem, varHeu, valHeu);
            }

        }

        public static void compareMethodsByDifficulty(CSP<T>[] problems, VariableHeuristics<T> varHeu = null, ValueHeuristics<T> valHeu = null)
        {
            if (varHeu == null) varHeu = new ByDefinitionHeuristics<T>();
            if (valHeu == null) valHeu = new ByDefinitionValueHeuristics<T>();

            var file = new System.IO.StreamWriter(FOLDERPATH + "porownanieAlgorytmow" + "wszystkieSudoku"+varHeu.ToString()+valHeu.ToString()+5+".txt");
            file.AutoFlush = true;

            file.WriteLine("Backtracking");

            string startLine = "IdProbl; Trudnosc; SredniczasBT; SredniczasFC; WezlyBT; WezlyFC";
            file.WriteLine(startLine);

            foreach (CSP<T> problem in problems)
            {
                float BTnodes = 0;
                float BTtime = 0;
                float FCnodes = 0;
                float FCtime = 0;

                for (int i=0; i<10; i++)
                {
                    CSPSolver<T> algBacktracking = new CSPSolver<T>(varHeu, valHeu);
                    algBacktracking.solve(problem);
                    BTnodes += algBacktracking.NumOfChecks;
                    BTtime += algBacktracking.TimeInMs;

                    CSPSolver<T> algForwardchecking = new CSPSolver<T>(varHeu, valHeu);
                    algForwardchecking.solveForward(problem);
                    FCnodes += algForwardchecking.NumOfChecks;
                    FCtime += algForwardchecking.TimeInMs;
                }

                string line = problem.Id + ";" + problem.Difficulty + ";" + BTtime / 5 + ";" + BTnodes / 5 + ";" + FCtime / 5 + ";" + FCnodes/5 + ";";
                file.WriteLine(line);
            }


            


        }

        public static void compareVariableHeuristicsExperiment(CSP<T> problem, VariableHeuristics<T>[] varHeuArray= null, ValueHeuristics<T> valHeu = null)
        {
            Console.WriteLine("Problem "+problem.Id);
            if (valHeu == null) valHeu = new ByDefinitionValueHeuristics<T>();

            var file = new System.IO.StreamWriter(FOLDERPATH + "porownanieHeurystykZmiennej" + problem.GetType() + problem.Id + valHeu.ToString() + ".txt");
            file.AutoFlush = true;
            //to do:
            string startLine = "VarHeu; ValHeu;  BTCzas; BTWezly; BTNawroty; BT Czas do 1; BTNawroty do 1;  BTWezly do 1; FCCzas; FCWezly; FC Nawroty; FC Czas do 1; FC Wezly do 1; FC Nawroty do 1; lRozw";
            file.WriteLine(startLine);

            foreach (VariableHeuristics<T> varHeu in varHeuArray)
            {
                Console.WriteLine(varHeu.ToString());
                float BTnodes = 0;
                float BTbacks = 0;
                float BTtime = 0;
                float BTnodes1 = 0;
                float BTbacks1 = 0;
                float BTtime1 = 0;
                float FCnodes = 0;
                float FCbacks = 0;
                float FCtime = 0;
                float FCnodes1 = 0;
                float FCbacks1 = 0;
                float FCtime1 = 0;
                int numOfSol = 0;

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(i);
                    CSPSolver<T> algBacktracking = new CSPSolver<T>(varHeu, valHeu);
                    algBacktracking.solve(problem);
                    BTnodes += algBacktracking.NumOfChecks;
                    BTtime += algBacktracking.TimeInMs;
                    BTbacks += algBacktracking.Backs;
                    BTnodes1 += algBacktracking.NumOfChecksFirstSol;
                    BTtime1 += algBacktracking.TimeFirstSol;
                    BTbacks1 += algBacktracking.BacksFirstSol;


                    CSPSolver<T> algForwardchecking = new CSPSolver<T>(varHeu, valHeu);
                    algForwardchecking.solveForward(problem);
                    FCnodes += algForwardchecking.NumOfChecks;
                    FCtime += algForwardchecking.TimeInMs;
                    FCbacks += algForwardchecking.Backs;
                    FCnodes1 += algForwardchecking.NumOfChecksFirstSol;
                    FCtime1 += algForwardchecking.TimeFirstSol;
                    FCbacks1 += algForwardchecking.BacksFirstSol;
                    numOfSol = algBacktracking.Solutions.Count;
                }

                string line = varHeu.ToString()+";"+valHeu.ToString()+";" + BTtime / 10 + ";" + BTnodes / 10 + ";"+BTbacks/10+";"+ BTtime1 / 10 + ";" + BTnodes1 / 10 + ";" + BTbacks1/10 + ";" + FCtime / 10 + ";" + FCnodes / 10 + ";"+FCbacks+";"+ FCtime1 / 10 + ";" + FCnodes1 / 10 + ";" + FCbacks1/10 + ";"+numOfSol+";";
                file.WriteLine(line);
            }
            file.Close();

        }

        public static void compareVariableExperimentForArray(CSP<T>[] problems, VariableHeuristics<T>[] varHeu = null, ValueHeuristics<T> valHeu = null)
        {
            foreach (CSP<T> problem in problems)
            {
                compareVariableHeuristicsExperiment(problem, varHeu, valHeu);
            }

        }

        public static void compareValueHeuristicsExperiment(CSP<T> problem, ValueHeuristics<T>[] valHeuArray = null, VariableHeuristics<T> varHeu = null)
        {
            Console.WriteLine("Problem " + problem.Id);
            if (varHeu == null) varHeu = new ByDefinitionHeuristics<T>();

            var file = new System.IO.StreamWriter(FOLDERPATH + "porownanieHeurystykWartosci" + problem.GetType() + problem.Id + varHeu.ToString() + ".txt");
            file.AutoFlush = true;
            //to do:
            string startLine = "VarHeu; ValHeu;  BTCzas; BTWezly; BTNawroty; BT Czas do 1; BTNawroty do 1;  BTWezly do 1; FCCzas; FCWezly; FC Nawroty; FC Czas do 1; FC Wezly do 1; FC Nawroty do 1; lRozw";
            file.WriteLine(startLine);

            foreach (ValueHeuristics<T> valHeu in valHeuArray)
            {
                Console.WriteLine(valHeu.ToString()) ;
                float BTnodes = 0;
                float BTbacks = 0;
                float BTtime = 0;
                float BTnodes1 = 0;
                float BTbacks1 = 0;
                float BTtime1 = 0;
                float FCnodes = 0;
                float FCbacks = 0;
                float FCtime = 0;
                float FCnodes1 = 0;
                float FCbacks1 = 0;
                float FCtime1 = 0;
                int numOfSol = 0;

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(i);
                    CSPSolver<T> algBacktracking = new CSPSolver<T>(varHeu, valHeu);
                    algBacktracking.solve(problem);
                    BTnodes += algBacktracking.NumOfChecks;
                    BTtime += algBacktracking.TimeInMs;
                    BTbacks += algBacktracking.Backs;
                    BTnodes1 += algBacktracking.NumOfChecksFirstSol;
                    BTtime1 += algBacktracking.TimeFirstSol;
                    BTbacks1 += algBacktracking.BacksFirstSol;


                    CSPSolver<T> algForwardchecking = new CSPSolver<T>(varHeu, valHeu);
                    algForwardchecking.solveForward(problem);
                    FCnodes += algForwardchecking.NumOfChecks;
                    FCtime += algForwardchecking.TimeInMs;
                    FCbacks += algForwardchecking.Backs;
                    FCnodes1 += algForwardchecking.NumOfChecksFirstSol;
                    FCtime1 += algForwardchecking.TimeFirstSol;
                    FCbacks1 += algForwardchecking.BacksFirstSol;
                    numOfSol = algBacktracking.Solutions.Count;
                }

                string line = varHeu.ToString() + ";" + valHeu.ToString() + ";" + BTtime / 10 + ";" + BTnodes / 10 + ";" + BTbacks / 10 + ";" + BTtime1 / 10 + ";" + BTnodes1 / 10 + ";" + BTbacks1 / 10 + ";" + FCtime / 10 + ";" + FCnodes / 10 + ";" + FCbacks + ";" + FCtime1 / 10 + ";" + FCnodes1 / 10 + ";" + FCbacks1 / 10 + ";" + numOfSol + ";";
                file.WriteLine(line);
            }
            file.Close();

        }

        public static void compareValueExperimentForArray(CSP<T>[] problems, ValueHeuristics<T>[] valHeu = null, VariableHeuristics<T> varHeu = null)
        {
            foreach (CSP<T> problem in problems)
            {
                compareValueHeuristicsExperiment(problem, valHeu, varHeu);
            }

        }
    }
}
