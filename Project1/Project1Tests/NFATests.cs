using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Project1.Tests
{
    [TestClass()]
    public class NFATests
    {
        [TestMethod()]
        public void getStartStatesTest()
        {
            string path = @"TestData\1.txt";
            var expected = new HashSet<int>();
            expected.Add(0);
            expected.Add(2);
            expected.Add(3);

            var nfa = new NFA(path);
            Assert.IsTrue(expected.SetEquals(nfa.getStartStates()));
        }
    }
}
