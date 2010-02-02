using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Compiler
{
    public interface IEmitter 
    {
        Section Section(SectionType type);

        void Emit(MethodDefinition methodDefinition);
        void Emit(MethodBody methodBody);
    }
}


