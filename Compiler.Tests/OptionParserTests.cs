using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Compiler.Tests
{
    [TestFixture]
    public class OptionParserTests
    {
        private class Options
        {
            public bool A { get; set; }
            public bool B { get; set; }
            public bool C { get; set; }
            public string D { get; set; }
        }

        [Test]
        [Row("/a", "/b")]
        [Row("-a", "-b")]
        [Row("--a", "--b")]
        [Row("--alpha", "--bravo")]
        public void Test1(string arg1, string arg2)
        {
            var options = new Options();
            var unparsed = RunParser(options, new[]{arg1, arg2});
            Assert.AreEqual(0, unparsed.Length);
            Assert.IsTrue(options.A);
            Assert.IsTrue(options.B);
            Assert.IsFalse(options.C);
            Assert.IsNull(options.D);
        }

        [Test]
        [Row("/a", "/d=top", null)]
        [Row("-a", "-d=top", null)]
        [Row("-alpha", "-delta=top", null)]
        [Row("--alpha", "--delta=top", null)]
        [Row("/alpha", "/delta=top", null)]
        [Row("/a", "/d", "top")]
        [Row("-a", "-d", "top")]
        [Row("-alpha", "-delta", "top")]
        [Row("--alpha", "--delta", "top")]
        [Row("/alpha", "/delta", "top")]
        public void Test2(string arg1, string arg2, string arg3)
        {
            var args = arg3 == null ? new[] {arg1, arg2} : new[] {arg1, arg2, arg3};
            var options = new Options();
            var unparsed = RunParser(options, args);
            Assert.AreEqual(0, unparsed.Length);
            Assert.IsTrue(options.A);
            Assert.IsFalse(options.B);
            Assert.IsFalse(options.C);
            Assert.AreEqual("top", options.D);
        }

        [Test]
        public void GetUsageTests()
        {
            var options = new Options();
            var parser = new OptionParser
                             {
                                 new Option {ShortForm = "a", LongForm = "alpha", Required= true, Action = () => options.A = true, Description = "Set the alpha standing"},
                                 new Option {ShortForm = "b", LongForm = "bravo", Action = () => options.B = true, Description = "Set the bravo standing"},
                                 new Option {ShortForm = "c", LongForm = "charlie", Action = () => options.C = true, Description = "Set the charlie standing"},
                                 new Option {ShortForm = "d", LongForm = "delta", ActionWithParam = param => options.D = param, Description = "Set the delta standing"},
                             };

            Assert.Inconclusive(parser.GetUsage());

        }

        [Test]
        [Row("/d")]
        [Row("-d")]
        [Row("--d")]
        [Row("/delta")]
        [Row("-delta")]
        [Row("--delta")]
        [ExpectedException(typeof(MissingOptionParameterException))]
        public void MissingOptionParameterTest(string arg)
        {
            var options = new Options();
            RunParser(options, "/a", arg);
        }

        [Test]
        [Row("/b")]
        [Row("-b")]
        [Row("--b")]
        [Row("/bravo")]
        [Row("-bravo")]
        [Row("--bravo")]
        [ExpectedException(typeof(MissingOptionException))]
        public void MissingOptionTest(string arg)
        {
            var options = new Options();
            RunParser(options, arg);
        }

        private static string[] RunParser(Options options,params string[] args)
        {
            var parser = new OptionParser
                             {
                                 new Option {ShortForm = "a", LongForm = "alpha", Required= true, Action = () => options.A = true},
                                 new Option {ShortForm = "b", LongForm = "bravo", Action = () => options.B = true},
                                 new Option {ShortForm = "c", LongForm = "charlie", Action = () => options.C = true},
                                 new Option {ShortForm = "d", LongForm = "delta", ActionWithParam = param => options.D = param},
                             };

            return parser.Parse(args);
            
        }
    }
}
