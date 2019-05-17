using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace Project1
{
    public class NFA
    {
        string[] alphabet;
        HashSet<int>[] adjacencyList;
        List<char>[,] cost;
        int startState;
        HashSet<int> finalStates;

        public NFA(string path)
        {
            string[] fileLines = File.ReadAllLines(path);
            int stateCount = int.Parse(fileLines[0]);
            // earn alphabet 
            this.alphabet = fileLines[1].Split(' ', ',');

            // initialize adjacencyList 
            this.adjacencyList = new HashSet<int>[stateCount];
            for (int i = 0; i < adjacencyList.Length; i++)
                this.adjacencyList[i] = new HashSet<int>();

            // initialize cost
            this.cost = new List<char>[stateCount, stateCount];
            for (int i = 0; i < stateCount; i++)
                for (int j = 0; j < stateCount; j++)
                    this.cost[i,j] = new List<char>();
            // determine startState and finalStates
            this.startState = 0;
            this.finalStates = new HashSet<int>();
            // create adjacency list
            for (int i = 2; i < fileLines.Length; i++)
            {
                string[] edge = fileLines[i].Split(',');
                int s = int.Parse(edge[0][edge[0].Length - 1].ToString());
                int t = int.Parse(edge[2][edge[2].Length - 1].ToString());

                if (edge[0][0] == '-')
                    this.startState = s;
                if (edge[0][0] == '*')
                    this.finalStates.Add(s);
                if (edge[2][0] == '-')
                    this.startState = t;
                if (edge[2][0] == '*')
                    this.finalStates.Add(t);
                this.adjacencyList[s].Add(t);
                this.cost[s, t].Add(edge[1][0]);
            }
        }
    }
}
