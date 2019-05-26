
using System.Collections.Generic;

using System.IO;
using System.Linq;

namespace Project1
{
    public class NFA
    {

        public int StateCount;
        public char[] Alphabet;
        public HashSet<int>[] AdjacencyList;
        public HashSet<char>[,] Cost;
        public int StartState;
        public HashSet<int> FinalStates;


        public NFA(string path)
        {
            string[] fileLines = File.ReadAllLines(path);

            StateCount = int.Parse(fileLines[0]);

            // get alphabet 
            this.Alphabet = fileLines[1].Split(' ', ',').Select(s => s[0]).ToArray();

            // initialize AdjacencyList 
            this.AdjacencyList = new HashSet<int>[StateCount];

            for (int i = 0; i < AdjacencyList.Length; i++)
                this.AdjacencyList[i] = new HashSet<int>();

            // initialize Cost

            this.Cost = new HashSet<char>[StateCount, StateCount];
            for (int i = 0; i < StateCount; i++)
                for (int j = 0; j < StateCount; j++)
                    this.Cost[i, j] = new HashSet<char>();

            // determine StartState and FinalStates
            this.StartState = 0;
            this.FinalStates = new HashSet<int>();

            // create Adjacencylist and Cost

            for (int i = 2; i < fileLines.Length; i++)
            {
                string[] edge = fileLines[i].Split(',');
                int s = int.Parse(edge[0][edge[0].Length - 1].ToString());
                int t = int.Parse(edge[2][edge[2].Length - 1].ToString());

                if (edge[0][0] == '-')
                    this.StartState = s;
                if (edge[0][0] == '*')
                    this.FinalStates.Add(s);
                if (edge[2][0] == '-')
                    this.StartState = t;
                if (edge[2][0] == '*')
                    this.FinalStates.Add(t);
                this.AdjacencyList[s].Add(t);
                this.Cost[s, t].Add(edge[1][0]);
            }
        }

        public DFA ToDFA()
        {
            var DFAStates = new List<HashSet<int>>();
            var DFAstartState = GetStartStates();
            DFAStates.Add(DFAstartState);
            var DFAadjList = new List<HashSet<int>>();

            const int N = 100;
            var DFACost = new HashSet<char>[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    DFACost[i, j] = new HashSet<char>();

            int index = 0;
            int trap = 0;
            bool thereIsTrap = false;
            do
            {
                var current = DFAStates[index];
                DFAadjList.Add(new HashSet<int>());
                foreach (var symbol in this.Alphabet)
                {
                    var newState = new HashSet<int>();
                    foreach (var state in current)
                    {
                        var reachableStates = GetReachableStates(symbol, state);
                        newState.UnionWith(reachableStates);
                    }
                    int to = AddNewState(ref newState, ref DFAStates);
                    DFAadjList[index].Add(to);
                    DFACost[index, to].Add(symbol);

                    if (newState.Count == 0)
                    {
                        thereIsTrap = true;
                        trap = to;
                    }
                }
                index++;
            } while (index < DFAStates.Count);

            //handle trap state
            if (thereIsTrap)
            {
                DFAadjList[trap].Add(trap);
                DFACost[trap, trap].UnionWith(this.Alphabet);
            }

            var DFAFinalStates = GetDFAFinalStates(DFAStates, this.FinalStates);
            var c = formatCost();
            return new DFA(DFAStates.Count, this.Alphabet, DFAadjList.ToArray(), c, 0, DFAFinalStates);

            HashSet<char>[,] formatCost()
            {
                int sCount = DFAStates.Count;
                var ans = new HashSet<char>[sCount, sCount];
                for (int i = 0; i < sCount; i++)
                    for (int j = 0; j < sCount; j++)
                    {
                        ans[i, j] = new HashSet<char>();
                        ans[i, j].UnionWith(DFACost[i, j]);
                    }
                return ans;
            }
        }
        public static HashSet<int> GetDFAFinalStates(List<HashSet<int>> states, HashSet<int> finalStates)
        {
            var ans = new HashSet<int>();
            for (int i = 0; i < states.Count; i++)
                foreach (var f in finalStates)
                    if (states[i].Contains(f))
                        ans.Add(i);
            return ans;
        }

        public static int AddNewState(ref HashSet<int> s, ref List<HashSet<int>> states)
        {
            for (int i = 0; i < states.Count; i++)
                if (s.SetEquals(states[i]))
                    return i;
            states.Add(s);
            return states.Count - 1;
        }

        public HashSet<int> GetReachableStates(char symbol, int state)
        {
            var ans = new HashSet<int>();

            var canGoToWithSymbol = new HashSet<int>();
            foreach (int i in AdjacencyList[state])
                if (Cost[state, i].Contains(symbol))
                    canGoToWithSymbol.Add(i);

            ans.UnionWith(canGoToWithSymbol);
            foreach (int i in canGoToWithSymbol)
                ans.UnionWith(this.CanGoWithLambda(i));

            return ans;
        }

        public HashSet<int> GetStartStates()
        {
            var ans = new HashSet<int>();
            ans.Add(this.StartState);
            ans.UnionWith(this.CanGoWithLambda(StartState));
            return ans;
        }

        public HashSet<int> CanGoWithLambda(int s)
        {
            var ans = new HashSet<int>();
            var queue = new Queue<long>();
            queue.Enqueue(s);

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();

                foreach (int i in AdjacencyList[current])
                    if (!ans.Contains(i) && Cost[current, i].Contains('_'))
                    {
                        queue.Enqueue(i);
                        ans.Add(i);
                    }
            }
            return ans;
        }
    }
}
