using System.Collections.Generic;

namespace Project1
{
    public class DFA
    {
        public int StateCount;
        public char[] Alphabet;
        public HashSet<int>[] AdjacencyList;
        public List<char>[,] Cost;
        public int StartState;
        public HashSet<int> FinalStates;

        public DFA() { }
        public DFA(
            int stateCount,
            char[] alphabet,
            HashSet<int>[] adjacencyList,
            List<char>[,] cost,
            int startState,
            HashSet<int> finalStates
            )
        {
            this.StateCount = stateCount;
            this.Alphabet = alphabet;
            this.AdjacencyList = adjacencyList;
            this.Cost = cost;
            this.StartState = startState;
            this.FinalStates = finalStates;
        }
    }
}
