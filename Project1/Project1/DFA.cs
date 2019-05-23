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

        public string ToRegex()
        {
            var B = new StringBuilder[this.StateCount];
            var A = new StringBuilder[this.StateCount, this.StateCount];
            initA(); initB();

            for (int n = StateCount - 1; n >= 0; n--)
            {
                B[n] = new StringBuilder(concat(star(A[n, n]), B[n].ToString()));
                for (int j = 0; j < n; j++)
                    A[n, j] = new StringBuilder(concat(star(A[n, n]), B[n].ToString()));
                for(int i=0;i<n;i++)
                {
                    B[i].Append('+' + concat(A[i,n].ToString(),B[n].ToString()));
                    for (int j = 0; j < n; j++)
                        A[i, j].Append('+' + concat(A[i,n].ToString(),A[n,j].ToString()));
                }
            }
            return B[0].ToString();

            void initA()
            {
                for (int i = 0; i < StateCount; i++)
                    for (int j = 0; j < StateCount; j++)
                    {
                        A[i, j] = new StringBuilder();
                        foreach (char s in Alphabet)
                            if (Cost[i, j].Contains(s))
                            {
                                if (A[i, j].Length == 0)
                                    A[i, j].Append(s);
                                else
                                {
                                    A[i, j].Append('+');
                                    A[i, j].Append(s);
                                }
                            }
                        if (A[i, j].Length == 0)
                            A[i, j].Append('$');
                    }
            }

            void initB()
            {
                for (int i = 0; i < StateCount; i++)
                {
                    B[i] = new StringBuilder();
                    if (FinalStates.Contains(i))
                        B[i].Append('_');
                    else
                        B[i].Append('$');
                }
            }

            string star(StringBuilder c)
            {
                if (c.ToString() == "$")
                    return "$";
                if (c.ToString() == "_")
                    return "_";
                return '(' + c.ToString() + ')' + '*';
            }

            string concat(string a, string b)
            {
                if (a == b)
                    return a;

                if (b == '(' + a + ')' + '*')
                    return b;
                if (a == '(' + b + ')' + '*')
                    return a;

                if (a == "$") return b;
                if (b == "$") return a;
                if (a == "_") return b;
                if (b == "_") return a;
                return a + b;
            }
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
