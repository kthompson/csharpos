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
            Assert.AreEqual("0", CompileAndRunMethod(() => 0));
            Assert.AreEqual("1", CompileAndRunMethod(() => 1));
            Assert.AreEqual("-1", CompileAndRunMethod(() => -1));
            Assert.AreEqual("10", CompileAndRunMethod(() => 10));
            Assert.AreEqual("-10", CompileAndRunMethod(() => -10));
            Assert.AreEqual("2736", CompileAndRunMethod(() => 2736));
            Assert.AreEqual("-2736", CompileAndRunMethod(() => -2736));
            Assert.AreEqual("536870911", CompileAndRunMethod(() => 536870911));
            Assert.AreEqual("-536870912", CompileAndRunMethod(() => -536870912));
            Assert.AreEqual("-5368709121234", CompileAndRunMethod(() => -5368709121234L));
            Assert.AreEqual("429496121113456735", CompileAndRunMethod(() => 429496121113456735L));
        }

        [Test]
        public void Integers()
        {

            Assert.AreEqual("0", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("1", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-1", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, -1);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-1", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_M1);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("10", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, 10);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-10", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, -10);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("2736", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, 2736);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-2736", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, -2736);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("536870911", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, 536870911);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-536870912", CompileAndRunMethod<int>(il =>
            {
                il.Emit(OpCodes.Ldc_I4, -536870912);
                il.Emit(OpCodes.Ret);
            }));
        }

        [Test]
        public void Longs()
        {
            Assert.AreEqual("-5368709121234", CompileAndRunMethod<long>(il =>
            {
                il.Emit(OpCodes.Ldc_I8, -5368709121234L);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("429496121113456735", CompileAndRunMethod<long>(il =>
            {
                il.Emit(OpCodes.Ldc_I8, 429496121113456735L);
                il.Emit(OpCodes.Ret);
            }));
        }

        [Test]
        public void Boolean()
        {
            Assert.AreEqual("True", CompileAndRunMethod<bool>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("False", CompileAndRunMethod<bool>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Ret);
            }));
        }


        [Test]
        public void Characters()
        {
            Assert.AreEqual("a", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)'a');
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("b", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)0x62);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("c", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)0x63);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("d", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)0x64);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("e", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)0x65);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("f", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)0x66);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("0", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)0x30);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("1", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)0x31);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("2", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)0x32);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("3", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)0x33);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("4", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)0x34);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("5", CompileAndRunMethod<char>(il =>
            {
                il.Emit(OpCodes.Ldc_I4_S, (sbyte)0x35);
                il.Emit(OpCodes.Ret);
            }));
        }

        [Test]
        public void Floats()
        {
            Assert.AreEqual("3.140", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, 3.14f);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-3.140", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, -3.14f);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("-9.750", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, -9.75f);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("9.750", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, 9.75f);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("0.141", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, 0.141234f);
                il.Emit(OpCodes.Ret);
            }));

            Assert.AreEqual("1233.114", CompileAndRunMethod<float>(il =>
            {
                il.Emit(OpCodes.Ldc_R4, 1233.114f);
                il.Emit(OpCodes.Ret);
            }));
        }

        
    }
}
