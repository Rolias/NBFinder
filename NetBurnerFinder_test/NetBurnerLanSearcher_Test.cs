using System.Threading;
using NetBurnerFinder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Collections.Generic;

namespace NetBurnerFinder_test
{


    [TestClass()]
    public class NetBurnerLanSearcher_Test
    {


        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion



        [TestMethod()]
        public void FoundDevices_Test()
        {
            NBFinder target = new NBFinder(); // TODO: Initialize to an appropriate value
          
            Thread.Sleep(5000);
            List<IPAddress> actual = target.FoundDevices;
            Assert.IsTrue(actual.Count > 0);
        }
    }
}
