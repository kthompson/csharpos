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
             Assert.AreEqual("False", CompileAndRunMethod(Expression.Lambda<Func<bool>>(Expression.Not(Expression.Constant(true, typeof(bool)))).Compile()));
             Assert.AreEqual("True", CompileAndRunMethod(Expression.Lambda<Func<bool>>(Expression.Not(Expression.Constant(false, typeof(bool)))).Compile()));
         }

         [Test]
         public void UnaryBitwiseNotExpression()
         {
             Assert.AreEqual("15", CompileAndRunMethod(() => ~0xfffffff0));
             Assert.AreEqual("4294967280", CompileAndRunMethod(() => ~0xf));
         }
    }
}
