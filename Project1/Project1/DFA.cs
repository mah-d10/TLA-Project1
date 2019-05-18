using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Project1
{
    public class DFA
    {
        public int StateCount;
        public char[] Alphabet;
        public HashSet<int>[] AdjacencyList;
        public HashSet<char>[,] Cost;
        public int StartState;
        public HashSet<int> FinalStates;
        public DFA(
            int stateCount,
            char[] alphabet,
            HashSet<int>[] adjacencyList,
            HashSet<char>[,] cost,
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

        public void Print(string outputPath)
        {
            var s = new StringBuilder();
            s.Append(this.StateCount); s.Append("\n");
            for (int i = 0; i < this.Alphabet.Length; i++)
            {
                s.Append(this.Alphabet[i]);
                if (i != this.Alphabet.Length - 1)
                    s.Append(',');
            }
            s.Append("\n");

            s.Append("->");
            for (int i = 0; i < StateCount; i++)
                for (int j = 0; j < StateCount; j++)
                    foreach (char symbol in Cost[i, j])
                    {
                        s.Append(getString(i)); s.Append(',');
                        s.Append(symbol); s.Append(',');
                        s.Append(getString(j)); s.Append("\n");
                    }

            //System.Console.WriteLine(s);
            File.WriteAllText(outputPath, s.ToString());
            return;

            string getString(int i)
            {
                var ans = new StringBuilder();
                if (FinalStates.Contains(i))
                    ans.Append('*');
                ans.Append('q'); ans.Append(i);
                return ans.ToString();
            }
        }
    }
}
