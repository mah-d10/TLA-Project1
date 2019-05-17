using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Project1.Tests
{
    [TestClass()]
    public class NFATests
    {
        [TestMethod()]
        public void NFATest()
        {
            string path = @"TestData\1.txt";
            var nfa = new NFA(path);
            Assert.AreEqual(4, nfa.StateCount);
            CollectionAssert.AreEqual(new char[] { 'a', 'b' }, nfa.Alphabet);
            Assert.AreEqual(0, nfa.StartState);
            Assert.IsTrue(nfa.FinalStates.SetEquals(new HashSet<int>() { 3 }));

            var adjListExpected = new HashSet<int>[] {
                new HashSet<int>() { 1, 2, 3 },
                new HashSet<int>() { 1, 3 },
                new HashSet<int>() { 3 },
                new HashSet<int>() { 1, 2 }
            };
            for (int i = 0; i < nfa.AdjacencyList.Length; i++)
                Assert.IsTrue(nfa.AdjacencyList[i].SetEquals(adjListExpected[i]));

            var costExpected = new List<char>[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    costExpected[i, j] = new List<char>();
            costExpected[0, 1].Add('a');
            costExpected[0, 1].Add('b');
            costExpected[0, 2].Add('_');
            costExpected[0, 3].Add('b');
            costExpected[1, 1].Add('a');
            costExpected[1, 3].Add('a');
            costExpected[2, 3].Add('_');
            costExpected[3, 1].Add('a');
            costExpected[3, 2].Add('b');
            for (int i = 0; i < nfa.StateCount; i++)
                for (int j = 0; j < nfa.StateCount; j++)
                    CollectionAssert.AreEqual(nfa.Cost[i, j], costExpected[i, j]);
        }

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

        [TestMethod()]
        public void AddNewStateTest()
        {
            var t1 = new HashSet<int>() { 1, 2, 3 };
            var t2 = new HashSet<int>() { 1, 4 };
            var t3 = new HashSet<int>() { 2, 3 };
            var t4 = new HashSet<int>() { 3, 4 };
            var states = new List<HashSet<int>> { t1, t2, t3, t4 };

            var newState = new HashSet<int>() { 1, 4 };
            Assert.IsFalse(NFA.AddNewState(ref newState, ref states));

            newState = new HashSet<int>() { 1, 3 };
            Assert.IsTrue(NFA.AddNewState(ref newState, ref states));
        }
    }
}
