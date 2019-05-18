namespace Project1
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Console.WriteLine(new string(new List<char>() { 'a', 'b' }.ToArray()));
            new NFA(@"..\..\..\TestData\1.txt").ToDFA().Print(@"..\..\..\TestData\out1.txt");
        }
    }
}
