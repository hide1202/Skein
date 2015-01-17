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
            @"{""key"":{""value"":""innerValue""}}",
            @"{""key"":{""value"":""innerValue""},""key2"":""value2""}",
            @"{""key"":{""value"":""innerValue""},""key2"":{""value2"":""innerValue2""}}",
        };

        [TestMethod]
        public void ValidCaseTest()
        {
            foreach (var testCase in _validCase)
                ParseTest(testCase);
        }

        public void ParseTest(params string[] testCase)
        {
            try
            {
                foreach (string json in testCase)
                {
                    var obj = JsonReader.Parse(json);
                    Debug.WriteLine("Success!!:\{obj.ToString()}");
                }
            }
            catch (JsonException e) { Assert.Fail(e.Message); }
        }
    }
}