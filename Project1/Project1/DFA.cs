using System.Collections.Generic;

namespace Project1
{
    public class DFA
    {
        string[] Alphabet;
        HashSet<int>[] AdjacencyList;
        char[,] Cost;
        int StartState;
        HashSet<int> FinalStates;

        public DFA()
        {

        }
    }
}
