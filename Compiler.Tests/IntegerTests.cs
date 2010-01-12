using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Translator;
using System.IO;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Compiler.Tests
{
    [TestFixture]
    public class IntegerTests : CompilerTest
    {
        [Test]
        public void IntegerDelegateTest()
        {

            Assert.AreEqual("0\r\n", CompileAndRunMethod(() => 0));
            Assert.AreEqual("1\r\n", CompileAndRunMethod(() => 1));
            Assert.AreEqual("-1\r\n", CompileAndRunMethod(() => -1));
            Assert.AreEqual("10\r\n", CompileAndRunMethod(() => 10));
            Assert.AreEqual("-10\r\n", CompileAndRunMethod(() => -10));
            Assert.AreEqual("2736\r\n", CompileAndRunMethod(() => 2736));
            Assert.AreEqual("-2736\r\n", CompileAndRunMethod(() => -2736));
            Assert.AreEqual("536870911\r\n", CompileAndRunMethod(() => 536870911));
            Assert.AreEqual("-536870912\r\n", CompileAndRunMethod(() => -536870912));
        }

        [Test]
        public void Integers()
        {

            Assert.AreEqual("0\r\n", CompileAndRunMethod(il =>
                                                             {
                                                                 il.Emit(OpCodes.Ldc_I4_0);
                                                                 il.Emit(OpCodes.Ret);  
                                                             }));

            Assert.AreEqual("1\r\n", CompileAndRunMethod(il =>
                                                             {
                                                                 il.Emit(OpCodes.Ldc_I4_1);
                                                                 il.Emit(OpCodes.Ret);
                                                             }));

            Assert.AreEqual("-1\r\n", CompileAndRunMethod(il =>
                                                              {
                                                                  il.Emit(OpCodes.Ldc_I4, -1);
                                                                  il.Emit(OpCodes.Ret);
                                                              }));

            Assert.AreEqual("10\r\n", CompileAndRunMethod(il =>
                                                              {
                                                                  il.Emit(OpCodes.Ldc_I4, 10);
                                                                  il.Emit(OpCodes.Ret);
                                                              }));

            Assert.AreEqual("-10\r\n", CompileAndRunMethod(il =>
                                                               {
                                                                   il.Emit(OpCodes.Ldc_I4, -10);
                                                                   il.Emit(OpCodes.Ret);
                                                               }));

            Assert.AreEqual("2736\r\n", CompileAndRunMethod(il =>
                                                                {
                                                                    il.Emit(OpCodes.Ldc_I4, 2736);
                                                                    il.Emit(OpCodes.Ret);
                                                                }));

            Assert.AreEqual("-2736\r\n", CompileAndRunMethod(il =>
                                                                 {
                                                                     il.Emit(OpCodes.Ldc_I4, -2736);
                                                                     il.Emit(OpCodes.Ret);
                                                                 }));

            Assert.AreEqual("536870911\r\n", CompileAndRunMethod(il =>
                                                                     {
                                                                         il.Emit(OpCodes.Ldc_I4, 536870911);
                                                                         il.Emit(OpCodes.Ret);
                                                                     }));

            Assert.AreEqual("-536870912\r\n", CompileAndRunMethod(il =>
                                                                      {
                                                                          il.Emit(OpCodes.Ldc_I4, -536870912);
                                                                          il.Emit(OpCodes.Ret);
                                                                      }));
        }

        private string CompileAndRunMethod(Action<CilWorker> action)
        {
            return CompileAndRunMethod(GenerateMethod(action));
        }

        private string CompileAndRunMethod(Func<int> action)
        {
            return CompileAndRunMethod(GenerateMethod(action));
        }

        private MethodDefinition GenerateMethod(Action<CilWorker> action)
        {
            var type = GenerateType();
            var method = new MethodDefinition(RandomString("TestMethod"), MethodAttributes.Static | MethodAttributes.Public, GetCorlibType<int>());
            action(method.Body.CilWorker);
            type.Methods.Add(method);
            return method;
        }

        private MethodDefinition GenerateMethod(Func<int> action)
        {
            var method = this.Assembly.MainModule.Import(action.Method).Resolve();
            method.Name = RandomString("TestMethod");
            method.Body.Simplify();
            return method;
        }
    }
}
