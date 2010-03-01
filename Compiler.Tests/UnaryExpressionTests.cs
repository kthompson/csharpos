using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using System.Linq.Expressions;

namespace Compiler.Tests
{
     [TestFixture]
    public class UnaryExpressionTests : CompilerTest
    {
         [Test]
         public void UnaryLogicalNotExpression()
         {


             Assert.AreEqual("False", CompileAndRunMethod(() =>
                                                              {
                                                                  var fieldT = true;
                                                                  return !fieldT;
                                                              }));
             Assert.AreEqual("True", CompileAndRunMethod(() =>
                                                             {
                                                                 var fieldF = false;
                                                                 return !fieldF;
                                                             }));
         }

         [Test]
         public void UnaryBitwiseNotExpression()
         {
             Assert.AreEqual("15", CompileAndRunMethod(() =>
             {
                 var field = 0xfffffff0;
                 return ~field;
             }));
             Assert.AreEqual("-16", CompileAndRunMethod(() =>
             {
                 var field = 0xf;
                 return ~field;
             }));
         }

         [Test]
         public void UnaryNegateExpression()
         {
             Assert.AreEqual("15", CompileAndRunMethod(() =>
             {
                 var field = -15;
                 return -field;
             }));
             Assert.AreEqual("-16", CompileAndRunMethod(() =>
             {
                 var field = 16;
                 return -field;
             }));
             Assert.AreEqual("0", CompileAndRunMethod(() =>
             {
                 var field = 0;
                 return -field;
             }));
             Assert.AreEqual("-21897", CompileAndRunMethod(() =>
             {
                 var field = 21897;
                 return -field;
             }));
             Assert.AreEqual("673", CompileAndRunMethod(() =>
             {
                 var field = -673;
                 return -field;
             }));
             Assert.AreEqual("-673", CompileAndRunMethod(() =>
             {
                 var field = 673;
                 return -field;
             }));


             Assert.AreEqual("-3.400", CompileAndRunMethod(() =>
             {
                 var field = 3.4f;
                 return -field;
             }));
         }
    }
}
