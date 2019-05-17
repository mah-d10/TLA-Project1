using System.Collections.Generic;
using System.Linq;

namespace Project1
{
    public class DFA
    {
        int StateCount { get { return this.AdjacencyList.Length; } }
        string[] Alphabet;
        HashSet<int>[] AdjacencyList;
        char[,] Cost;
        int StartState;
        HashSet<int> FinalStates;

        public DFA(
            string[] alphabet,
            HashSet<int>[] adjacencyList,
            char[,] cost,
            int startState,
            HashSet<int> finalStates
            )
        {
            this.Alphabet = alphabet;
            this.AdjacencyList = adjacencyList;
            this.Cost = cost;
            this.StartState = startState;
            this.FinalStates = finalStates;
        }

        public DFA Minimize()
        {
            // first determine unreachable states
            DFA minimizedDFA = new DFA(
                this.Alphabet,
                this.AdjacencyList,
                this.Cost,
                this.StartState,
                this.FinalStates
                );
            bool[] reachable = minimizedDFA.DetermineReachables();

            // remove reachAbles
            for (int i = 0; i < reachable.Length; i++)
            {
                if (reachable[i] == false)
                {
                    minimizedDFA.AdjacencyList[i] = null;
                }
            }

            bool[,] equality = new bool[StateCount, StateCount];


            for (int i = 1; i < StateCount && reachable[i] == true; i++)
            {
                for (int j = 0; j < StateCount - 1 && reachable[j] == true; j++)
                {
                    if (minimizedDFA.checkEquality(i, j) == 0)
                    {
                    }
                }
            }


            return minimizedDFA;
        }

        public int checkEquality(int node1, int node2)
        {
            for (int i = 0; i < this.Alphabet.Length; i++)
            {
            }
            return 0;
        }

        /// <summary>
        /// Determine reachability for states.
        /// ture --> reachable
        /// false --> unreachable
        /// </summary>
        /// <returns></returns>
        public bool[] DetermineReachables()
        {
            bool[] reachable = new bool[this.StateCount];
            Queue<int> q = new Queue<int>();

            q.Enqueue(StartState);
            reachable[StartState] = true;

            // BFS
            while (q.Count != 0)
            {
                int node = q.Dequeue();
                for (int i = 0; i < this.AdjacencyList[node].Count; i++)
                {
                    if (reachable[this.AdjacencyList[node].ElementAt(i)] == false)
                    {
                        q.Enqueue(this.AdjacencyList[node].ElementAt(i));
                        reachable[this.AdjacencyList[node].ElementAt(i)] = true;
                    }
                }
            }
            return reachable;
        }
    }
}
