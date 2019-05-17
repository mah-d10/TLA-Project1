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
        public List<char>[,] Cost;
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
            this.Cost = new List<char>[StateCount, StateCount];
            for (int i = 0; i < StateCount; i++)
                for (int j = 0; j < StateCount; j++)
                    this.Cost[i, j] = new List<char>();

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
            var DFAstartState = getStartStates();
            DFAStates.Add(DFAstartState);
            bool thereIsNewState = true;
            int index = 0;
            do
            {
                var current = DFAStates[index];
                thereIsNewState = false;
                foreach (var symbol in this.Alphabet)
                {
                    var newState = new HashSet<int>();
                    foreach (var state in current)
                    {
                        var reachableStates = getReachableStates(symbol, state);
                        newState.UnionWith(reachableStates);
                    }
                    thereIsNewState = AddNewState(ref newState, ref DFAStates);
                }
            } while (thereIsNewState);

            return new DFA();
        }

        public static bool AddNewState(ref HashSet<int> s, ref List<HashSet<int>> states)
        {
            foreach (var state in states)
                if (s.SetEquals(state))
                    return false;
            return true;
        }

        public HashSet<int> getReachableStates(char symbol, int state)
        {
            var ans = new HashSet<int>();

            var canGoToWithSymbol = new HashSet<int>();
            foreach (int i in AdjacencyList[state])
                if (Cost[state, i].Contains(symbol))
                    canGoToWithSymbol.Add(i);

            return ans;
        }

        public HashSet<int> getStartStates()
        {
            var ans = new HashSet<int>();
            ans.Add(this.StartState);
            var queue = new Queue<long>();
            queue.Enqueue(StartState);

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
