using System.Collections.Generic;
using System.IO;

namespace Project1
{
    public class NFA
    {
        public int StateCount;
        public string[] Alphabet;
        public HashSet<int>[] AdjacencyList;
        public List<char>[,] Cost;
        public int StartState;
        public HashSet<int> FinalStates;

        public NFA(string path)
        {
            string[] fileLines = File.ReadAllLines(path);
            StateCount = int.Parse(fileLines[0]);

            // get alphabet 
            this.Alphabet = fileLines[1].Split(' ', ',');

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

            // create Adjacencylist
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
            var newStates = new List<HashSet<int>>();
            var DFAstartState = getStartStates();

            return new DFA();
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
