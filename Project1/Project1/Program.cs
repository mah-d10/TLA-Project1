using System.Collections.Generic;
using System.IO;

namespace Project1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"X:\Git_Repos\TLA-Project1\1.txt";
            NFA nfa = new NFA(path);
        }
    }
}
