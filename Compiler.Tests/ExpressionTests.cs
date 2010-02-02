using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Linq.Expressions;

namespace Compiler.Tests
{
     [TestFixture]
    public class ExpressionTests : CompilerTest
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
             Assert.AreEqual("15", CompileAndRunMethod(() => ~0xfffffff0));
             Assert.AreEqual("4294967280", CompileAndRunMethod(() => ~0xf));
         }
    }
}
