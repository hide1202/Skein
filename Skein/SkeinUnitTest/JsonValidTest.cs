using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skein;
using Skein.Exception;
using System.Diagnostics;

namespace SkeinUnitTest
{
    [TestClass]
    public class JsonValidTest
    {
        private string[] _validCase = new string[] {
            @"{""key"":""value""}",
            @"{""key"":1234}",
            @"{""key"":1.234}",
            @"{""key"":-1.234}",
            @"{""key"":3000E+2}",
            @"{""key"":3000E-2}",
            @"{""key"":1.234E+2}",
            @"{""key"":1.234E-2}",
            @"{""key"":{""value"":""innerValue""}}",
            @"{""key"":{""value"":123456}}",
            @"{""key"":{""value"":""innerValue""},""key2"":""value2""}",
            @"{""key"":{""value"":""innerValue""},""key2"":{""value2"":""innerValue2""}}",
            @"{""key"":[1,2,3,4,5] }",
            @"{""key"":[1,2,""value"",{""key2"":[1,2,""value2""]},5] }",
        };

        private string[] _invalidCase = new string[] {
            @"{""key"":""1234}",
        };

        [TestMethod]
        public void ValidCaseTest()
        {
            foreach (var testCase in _validCase)
                try
                {
                    ParseTest(testCase);
                }
                catch (System.Exception e)
                {
                    Assert.Fail(e.Message);
                }

            foreach (var testCase in _invalidCase)
                try
                {
                    ParseTest(testCase);
                    Assert.Fail("Success to parse invalid case");
                }
                catch (System.Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

        }

        public void ParseTest(params string[] testCase)
        {
            foreach (string json in testCase)
            {
                var obj = JsonReader.Parse(json);
                Debug.WriteLine("Success:\{obj.ToLogString()}");
            }
        }
    }
}