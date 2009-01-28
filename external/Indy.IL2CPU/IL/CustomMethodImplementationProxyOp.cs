using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using Indy.IL2CPU.Plugs;
using Mono.Cecil;

namespace Indy.IL2CPU.IL
{
    public abstract class CustomMethodImplementationProxyOp : Op
    {
        public readonly MethodInformation MethodInfo;
        public CustomMethodImplementationProxyOp(Mono.Cecil.Cil.Instruction instruction, MethodInformation aMethodInfo)
            : base(instruction, aMethodInfo)
        {
            MethodInfo = aMethodInfo;
        }

        public MethodDefinition ProxiedMethod { get; private set; }

        protected abstract void Ldarg(int aIndex);
        protected abstract void Ldflda(TypeInformation aType, TypeInformation.Field aField);
        protected abstract void CallProxiedMethod();
        protected abstract void Ldloc(int index);

        public sealed override void DoAssemble()
        {
            bool isFirst = true;
            int curIndex = 0;
            ParameterDefinitionCollection xParams = ProxiedMethod.Parameters;
            foreach (ParameterDefinition xParam in xParams)
            {
                if (isFirst && (!MethodInfo.Method.IsStatic))
                {
                    isFirst = false;
                    Ldarg(curIndex++);
                }
                else
                {
                    var xFieldAccess = xParam.CustomAttributes.OfType<FieldAccessAttribute>().FirstOrDefault();
                    if (xFieldAccess != null)
                    {
                        Ldarg(0);
                        if (!MethodInfo.TypeInfo.Fields.ContainsKey(xFieldAccess.Name))
                        {
                            throw new Exception("Field '" + xFieldAccess.Name + "' not found!");
                        }
                        Ldflda(MethodInfo.TypeInfo, MethodInfo.TypeInfo.Fields[xFieldAccess.Name]);
                    }
                    else
                    {
                        Ldarg(curIndex++);
                    }
                }
            }
            CallProxiedMethod();
            OnQueueMethod(ProxiedMethod);
        }
    }
}