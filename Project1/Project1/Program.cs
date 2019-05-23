using System.Text.RegularExpressions;

namespace Project1
{
    class Program
    {
        static void Main(string[] args)
        {
            var dfa = new NFA(@"..\..\..\TestData\1.txt").ToDFA();
            var s = dfa.ToRegex();
            System.Console.WriteLine(s);
        }
    }
}
