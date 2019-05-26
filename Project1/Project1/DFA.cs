using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Project1
{
    public class DFA
    {
        public bool isMIN;
        public int StateCount;
        public char[] Alphabet;
        public HashSet<int>[] AdjacencyList;
        public HashSet<char>[,] Cost;
        public int StartState;
        public HashSet<int> FinalStates;
        Dictionary<int, Dictionary<char, int>> ADJList;

        public Dictionary<int, Dictionary<char, int>> ADJ;

        public DFA(
            int stateCount,
            char[] alphabet,
            Dictionary<int, Dictionary<char, int>> ADJ,
            int startState,
            HashSet<int> finalStates
            )
        {
            this.StateCount = startState;
            this.Alphabet = alphabet;
            this.ADJList = ADJ;
            this.StartState = startState;
            this.FinalStates = finalStates;
        }

        public DFA(
            int stateCount,
            char[] alphabet,
            HashSet<int>[] adjacencyList,
            HashSet<char>[,] cost,
            int startState,
            HashSet<int> finalStates
            )
        {
            this.isMIN = false;
            this.StateCount = stateCount;
            this.Alphabet = alphabet;
            this.AdjacencyList = adjacencyList;
            this.Cost = cost;
            this.StartState = startState;
            this.FinalStates = finalStates;

            // good data structure to achive transition in O(1).
            ADJ = new Dictionary<int, Dictionary<char, int>>();
            for (int i = 0; i < stateCount; i++)
            {
                ADJ.Add(i, new Dictionary<char, int>());
                for (int j = 0; j < stateCount; i++)
                {
                    if (Cost[i, j] != null)
                    {
                        foreach (char c in Cost[i, j])
                        {
                            ADJ[i].Add(c, j);
                        }
                    }
                }
                foreach (char c in alphabet)
                {
                    if (!ADJ[i].Keys.Contains(c))
                    {
                        ADJ[i].Add(c, -1);
                    }
                }
            }
        }

        public DFA Minimize()
        {
            // first determine unreachable states

            // filter: remove unreachables
            var reachables = DetermineReachables();

            // filter: remove nodes that dosent have path to any final sate.
            HashSet<int> minimizedStates = GetCanGoToFinalState(reachables);

            // divide nodes into two list : 0) non-final state   1) final state
            int lable = 0;
            int[] lables = new int[minimizedStates.Count];


            lable = 1;
            foreach (int s in minimizedStates)
            {
                if (FinalStates.Contains(s))
                {
                    lables[s] = lable;
                }
            }

            int clable = lable;
            lable++;
            while (true)
            {
                // if no new lable added
                if (clable == lable) break;
                clable = lable;
                for (int l = 0; l <= clable; l++)
                {
                    // extract nodes with same lables.
                    var temp = minimizedStates.Where(x => lables[x] == l);
                    bool flag = true;
                    for (int j = 0; j < temp.Count(); j++)
                    {
                        for (int i = j + 1; i < temp.Count(); i++)
                        {
                            if (!isEqual(temp.ElementAt(j), temp.ElementAt(i), lables))
                            {
                                if (flag)
                                {
                                    lable++;
                                    flag = false;
                                }
                                lables[temp.ElementAt(j)] = lable;
                                break;
                            }
                        }
                    }
                }
            }
            int SC = lables.GroupBy(x => x).Count();
            List<int> stateMin = minimizedStates.GroupBy(x => lables[x])
                                .Select(grp => grp.OrderBy(x => x).First()).OrderBy(x => x)
                                .ToList();

            lables = lables.GroupBy(x => x).Select(x => x.First()).ToArray();

            Dictionary<int, Dictionary<char, int>> minADJ = new Dictionary<int, Dictionary<char, int>>();
            for (int i = 0; i < lables.Count(); i++)
            {
                minADJ.Add(lables[i], new Dictionary<char, int>());
            }

            foreach (int s in minimizedStates)
            {
                for (int j = 0; j < Alphabet.Count(); j++)
                {
                    minADJ[lables[s]].Add(Alphabet[j], lables[ADJ[s][Alphabet[j]]]);
                }
            }

            HashSet<int> minFinalStates = new HashSet<int>();

            foreach (int s in minimizedStates)
            {
                if (FinalStates.Contains(s))
                {
                    minFinalStates.Add(lables[s]);
                }
            }

            return new DFA(minADJ.Count, Alphabet, minADJ, 0, minFinalStates);
        }


        public bool isEqual(int node1, int node2, int[] lables)
        {
            bool flag = true;
            for (int i = 0; i < this.Alphabet.Length; i++)
            {
                // check for same lable
                if (lables[ADJ[node1][Alphabet[i]]] != lables[ADJ[node2][Alphabet[i]]])
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }


        /// <summary>
        /// Determine reachability for states.
        /// ture --> reachable
        /// false --> unreachable
        /// </summary>
        /// <returns></returns>
        public HashSet<int> DetermineReachables()
        {
            bool[] reachable = new bool[this.StateCount];
            Queue<int> q = new Queue<int>();
            var ans = new HashSet<int>();

            q.Enqueue(StartState);
            reachable[StartState] = true;
            ans.Add(StartState);

            // BFS
            while (q.Count != 0)
            {
                int node = q.Dequeue();
                foreach (int i in AdjacencyList[node])
                {
                    if (reachable[i] == false)
                    {
                        q.Enqueue(i);
                        reachable[i] = true;
                        ans.Add(i);
                    }
                }
            }
            return ans;
        }

        public HashSet<int> GetCanGoToFinalState(HashSet<int> states)
        {
            var ans = new HashSet<int>();
            foreach (int s in states)
            {
                bool[] visited = new bool[states.Count];
                Queue<int> q = new Queue<int>();
                q.Enqueue(s);
                visited[s] = true;
                ans.Add(s);

                bool Continue = true;
                // BFS
                while (q.Count != 0 && Continue)
                {
                    int node = q.Dequeue();
                    foreach (int i in AdjacencyList[node])
                    {
                        if (visited[i] == false)
                        {
                            q.Enqueue(i);
                            visited[i] = true;
                            if (FinalStates.Contains(i))
                            {
                                ans.Add(s);
                                Continue = false;
                                break;
                            }
                        }
                    }
                }
            }
            return ans;
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
                for (int i = 0; i < n; i++)
                {
                    B[i].Append('+' + concat(A[i, n].ToString(), B[n].ToString()));
                    for (int j = 0; j < n; j++)
                        A[i, j].Append('+' + concat(A[i, n].ToString(), A[n, j].ToString()));
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
