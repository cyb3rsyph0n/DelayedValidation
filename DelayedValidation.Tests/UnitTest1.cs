using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DelayedValidation.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            SomeDomainObject s = new SomeDomainObject("first", "last");

            s.Age = 25;

            Assert.AreEqual(s.FirstName, "first");
            Assert.AreEqual(s.LastName, "last");
        }
    }
}
