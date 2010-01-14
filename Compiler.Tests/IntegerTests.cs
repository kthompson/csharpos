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
            Assert.AreEqual("-5368709121234\r\n", CompileAndRunMethod(() => -5368709121234L));
            Assert.AreEqual("429496121113456735\r\n", CompileAndRunMethod(() => 429496121113456735L));
        }

        [Test]
        public void Integers()
        {

            Assert.AreEqual("0\r\n", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("1\r\n", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-1\r\n", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, -1);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-1\r\n", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_M1);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("10\r\n", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, 10);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-10\r\n", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, -10);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("2736\r\n", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, 2736);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-2736\r\n", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, -2736);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("536870911\r\n", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, 536870911);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-536870912\r\n", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, -536870912);
                il.Emit(OpCodes.Ret);
            }));
        }

        [Test]
        public void Longs()
        {
            Assert.AreEqual("-5368709121234\r\n", CompileAndRunMethod<long>(il =>
            {
                il.Emit(OpCodes.Ldc_I8, -5368709121234L);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("429496121113456735\r\n", CompileAndRunMethod<long>(il =>
            {
                il.Emit(OpCodes.Ldc_I8, 429496121113456735L);
                il.Emit(OpCodes.Ret);
            }));
        }

        [Test]
        public void Floats()
        {
            Assert.AreEqual("3.140\r\n", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, 3.14f);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-3.140\r\n", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, -3.14f);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-9.750\r\n", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, -9.75f);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("9.750\r\n", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, 9.75f);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("0.141\r\n", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, 0.141234f);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("1233.114\r\n", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, 1233.114f);
                il.Emit(OpCodes.Ret);
            }));
        }

        
    }
}
