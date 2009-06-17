using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Translator
{
    public class MethodDependencies : BaseReflectionVisitor
    {
        public List<MethodDefinition> Dependencies { get; private set; }
        public List<AssemblyDefinition> Assemblies { get; private set; }

        public MethodDependencies()
        {
            this.Dependencies = new List<MethodDefinition>();
            this.Assemblies = new List<AssemblyDefinition>();
        }

        private void LoadAssemblies(MethodDefinition method)
        {
            var type = method.DeclaringType;
            var module = type.Module;
            var asm = module.Assembly;
            this.Assemblies.Add(asm);

            var resolver = asm.Resolver;
            foreach (AssemblyNameReference reference in module.AssemblyReferences)
		        this.Assemblies.Add(resolver.Resolve(reference));
        }

        private MethodDefinition[] ResolveMemberReference(MemberReference member)
        {
            var typeName = member.DeclaringType.FullName;
            foreach (var asm in this.Assemblies)
            {
                foreach (ModuleDefinition module in asm.Modules)
                {
                    var type = module.Types[typeName];
                    if (type == null)
                        continue;

                    return type.Methods.GetMethod(member.Name);
                }
            }
            return new MethodDefinition[]{};
        }

        private MethodDefinition ResolveMethodReference(MethodReference method)
        {
            var typeName = method.DeclaringType.FullName;
            foreach (var asm in this.Assemblies)
            {
                foreach (ModuleDefinition module in asm.Modules)
                {
                    var type = module.Types[typeName];
                    if (type == null)
                        continue;

                    return type.Methods.GetMethod(method.Name, method.Parameters);
                }
            }
            return null;
        }

        public override void VisitMethodDefinition(MethodDefinition method)
        {
            
            if (this.Assemblies.Count == 0) LoadAssemblies(method);
            
            if (this.Dependencies.Contains(method)) return;
  
            this.Dependencies.Add(method);

            if (method.HasBody)
            {
                foreach (Instruction instruction in method.Body.Instructions)
                {
                    if (instruction.Operand != null)
                    {
                        switch (instruction.OpCode.Code)
                        {
                            case Code.Call:
                            case Code.Callvirt:
                            case Code.Newobj:
                                break;
                            default:
                                continue;
                        }
                        var operand = instruction.Operand;
                        
                        if (operand is MethodReference)
                        {

                            var md = ResolveMethodReference(operand as MethodReference);
                            if (md != null)
                            {
                                VisitMethodDefinition(md);
                            }
                        }
                        else if (operand is MemberReference)
                        {
                            var mr = operand as MemberReference;
                            VisitMemberReference(mr);
                        }
                    }
                }
            }
        }



        public override void VisitMemberReference(MemberReference member)
        {
            var methods = ResolveMemberReference(member);
            foreach (var method in methods)
                VisitMethodDefinition(method);
        }
    }
}
