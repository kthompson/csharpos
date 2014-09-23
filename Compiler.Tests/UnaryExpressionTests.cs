using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Xunit;

namespace Compiler.Tests
{
    public class UnaryExpressionTests : CompilerTest
    {
         [Fact]
         public void UnaryLogicalNotExpression()
         {


             Assert.Equal("False", CompileAndRunMethod(() =>
                                                              {
                                                                  var fieldT = true;
                                                                  return !fieldT;
                                                              }));
             Assert.Equal("True", CompileAndRunMethod(() =>
                                                             {
                                                                 var fieldF = false;
                                                                 return !fieldF;
                                                             }));
         }

         [Fact]
         public void UnaryBitwiseNotExpression()
         {
             Assert.Equal("15", CompileAndRunMethod(() =>
             {
                 var field = 0xfffffff0;
                 return ~field;
             }));
             Assert.Equal("-16", CompileAndRunMethod(() =>
             {
                 var field = 0xf;
                 return ~field;
             }));
         }

         [Fact]
         public void UnaryNegateExpression()
         {
             Assert.Equal("15", CompileAndRunMethod(() =>
             {
                 var field = -15;
                 return -field;
             }));
             Assert.Equal("-16", CompileAndRunMethod(() =>
             {
                 var field = 16;
                 return -field;
             }));
             Assert.Equal("0", CompileAndRunMethod(() =>
             {
                 var field = 0;
                 return -field;
             }));
             Assert.Equal("-21897", CompileAndRunMethod(() =>
             {
                 var field = 21897;
                 return -field;
             }));
             Assert.Equal("673", CompileAndRunMethod(() =>
             {
                 var field = -673;
                 return -field;
             }));
             Assert.Equal("-673", CompileAndRunMethod(() =>
             {
                 var field = 673;
                 return -field;
             }));


             Assert.Equal("-3.400", CompileAndRunMethod(() =>
             {
                 var field = 3.4f;
                 return -field;
             }));
         }
    }
}
