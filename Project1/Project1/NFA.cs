using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Project1
{
    public class NFA
    {
        string[] Alphabet;
        HashSet<int>[] AdjacencyList;
        List<char>[,] Cost;
        int StartState;
        HashSet<int> FinalStates;

        public NFA(string path)
        {
            string[] fileLines = File.ReadAllLines(path);
            int stateCount = int.Parse(fileLines[0]);
            // earn Alphabet 
            this.Alphabet = fileLines[1].Split(' ', ',');

            // initialize AdjacencyList 
            this.AdjacencyList = new HashSet<int>[stateCount];
            for (int i = 0; i < AdjacencyList.Length; i++)
                this.AdjacencyList[i] = new HashSet<int>();

            // initialize Cost
            this.Cost = new List<char>[stateCount, stateCount];
            for (int i = 0; i < stateCount; i++)
                for (int j = 0; j < stateCount; j++)
                    this.Cost[i,j] = new List<char>();
            // determine StartState and FinalStates
            this.StartState = 0;
            this.FinalStates = new HashSet<int>();
            // create adjacency list
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
    }
}
