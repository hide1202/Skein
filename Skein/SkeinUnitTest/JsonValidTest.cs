using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skein;
using Skein.Exception;

namespace SkeinUnitTest
{
    [TestClass]
    public class JsonValidTest
    {
        [TestMethod]
        public void MainLogicIntegrationTest()
        {
            ParseTest(
                @"{""key"":""value""}"
            );
        }

        public void ParseTest(params string[] testCase)
        {
            try { foreach (string json in testCase) JsonReader.Parse(json); }
            catch (JsonException e) { Assert.Fail(e.Message); }
        }
    }
}