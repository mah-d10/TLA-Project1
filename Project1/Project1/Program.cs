using System.Collections.Generic;
using System.IO;

namespace Project1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\MAHDI\Desktop\1.txt";
            string[] fileLines = File.ReadAllLines(path);
            int stateCount = int.Parse(fileLines[0]);
            string[] alphabet = fileLines[1].Split(' ', ',');

            var adjacencyList = new HashSet<int>[stateCount];
            for (int i = 0; i < adjacencyList.Length; i++)
                adjacencyList[i] = new HashSet<int>();

            var cost = new List<char>[stateCount, stateCount];
            for (int i = 0; i < stateCount; i++)
                for (int j = 0; j < stateCount; j++)
                    cost[i, j] = new List<char>();

            int startState = 0;
            var finalStates = new HashSet<int>();
            for (int i = 2; i < fileLines.Length; i++)
            {
                string[] edge = fileLines[i].Split(',');
                int s = int.Parse(edge[0][edge[0].Length - 1].ToString());
                int t = int.Parse(edge[2][edge[2].Length - 1].ToString());

                if (edge[0][0] == '-')
                    startState = s;
                if (edge[0][0] == '*')
                    finalStates.Add(s);
                if (edge[2][0] == '-')
                    startState = t;
                if (edge[2][0] == '*')
                    finalStates.Add(t);
                adjacencyList[s].Add(t);
                cost[s, t].Add(edge[1][0]);
            }
        }
    }
}
