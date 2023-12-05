using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClearSpam.Common.Tests
{
    [TestClass]
    public class InvariantCultureComparerTests
    {
        [TestMethod]
        [DataRow("A", "A", 0)]
        [DataRow("a", "A", 0)]
        [DataRow("a", "a", 0)]
        [DataRow("a", "b", -1)]
        [DataRow("b", "a", 1)]
        public void Compare_HappyPath(string x, string y, int result)
        {
            var comparer = new InvariantCultureComparer();

            Assert.AreEqual(result, comparer.Compare(x, y));
        }
    }
}
