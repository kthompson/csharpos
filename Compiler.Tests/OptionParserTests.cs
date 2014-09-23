using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Compiler.Tests
{
    public class OptionParserTests
    {
        private class Options
        {
            public bool A { get; set; }
            public bool B { get; set; }
            public bool C { get; set; }
            public string D { get; set; }
        }

        [Theory]
        [InlineData("/a", "/b")]
        [InlineData("-a", "-b")]
        [InlineData("--a", "--b")]
        [InlineData("--alpha", "--bravo")]
        public void Test1(string arg1, string arg2)
        {
            var options = new Options();
            var unparsed = RunParser(options, new[]{arg1, arg2});
            Assert.Equal(0, unparsed.Length);
            Assert.True(options.A);
            Assert.True(options.B);
            Assert.False(options.C);
            Assert.Null(options.D);
        }

        [Theory]
        [InlineData("/a", "/d=top", null)]
        [InlineData("-a", "-d=top", null)]
        [InlineData("-alpha", "-delta=top", null)]
        [InlineData("--alpha", "--delta=top", null)]
        [InlineData("/alpha", "/delta=top", null)]
        [InlineData("/a", "/d", "top")]
        [InlineData("-a", "-d", "top")]
        [InlineData("-alpha", "-delta", "top")]
        [InlineData("--alpha", "--delta", "top")]
        [InlineData("/alpha", "/delta", "top")]
        public void Test2(string arg1, string arg2, string arg3)
        {
            var args = arg3 == null ? new[] {arg1, arg2} : new[] {arg1, arg2, arg3};
            var options = new Options();
            var unparsed = RunParser(options, args);
            Assert.Equal(0, unparsed.Length);
            Assert.True(options.A);
            Assert.False(options.B);
            Assert.False(options.C);
            Assert.Equal("top", options.D);
        }

        [Fact]
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

            
            parser.GetUsage();
        }

        [Theory]
        [InlineData("/d")]
        [InlineData("-d")]
        [InlineData("--d")]
        [InlineData("/delta")]
        [InlineData("-delta")]
        [InlineData("--delta")]
        public void MissingOptionParameterTest(string arg)
        {
            var options = new Options();
            Assert.Throws<MissingOptionParameterException>(() => RunParser(options, "/a", arg));
        }

        [Theory]
        [InlineData("/b")]
        [InlineData("-b")]
        [InlineData("--b")]
        [InlineData("/bravo")]
        [InlineData("-bravo")]
        [InlineData("--bravo")]
        public void MissingOptionTest(string arg)
        {
            var options = new Options();
            Assert.Throws<MissingOptionException>(() => RunParser(options, arg));
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
